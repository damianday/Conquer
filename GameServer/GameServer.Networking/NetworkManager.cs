using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using GameServer.Map;
using GameServer.Database;

using GamePackets;
using GamePackets.Server;

namespace GameServer.Networking;

public static class NetworkManager
{
    private static IPEndPoint TicketSenderEndPoint;

    private static UdpClient TicketListener;
    private static TcpListener Listener;
    private static TcpListener StatusListener;

    public static bool Stopped;
    private static bool StatusPortEnabled = true;

    public static uint ConnectionCount => (uint)Connections.Count;
    public static uint ActiveConnections { get; set; }
    public static uint ConnectionsOnline { get; set; }
    public static uint ConnectionsOnline1 { get; set; }
    public static uint ConnectionsOnline2 { get; set; }

    public static long TotalSentBytes;
    public static long TotalReceivedBytes;

    private static List<SConnection> Connections;
    private static ConcurrentQueue<SConnection> DisconnectingConnections;
    private static ConcurrentQueue<SConnection> ConnectingConnections;

    private static List<StatusConnection> StatusConnections = new List<StatusConnection>();
    private static ConcurrentQueue<StatusConnection> StatusDisconnectingConnections;
    private static ConcurrentQueue<StatusConnection> StatusConnectingConnections;

    private static ConcurrentQueue<GamePacket> ServerBroadcasts;

    public static ConcurrentDictionary<string, TicketInformation> Tickets;

    public static void StartService()
    {
        Stopped = false;

        Connections = new List<SConnection>();
        ConnectingConnections = new ConcurrentQueue<SConnection>();
        DisconnectingConnections = new ConcurrentQueue<SConnection>();

        StatusConnections = new List<StatusConnection>();
        StatusDisconnectingConnections = new ConcurrentQueue<StatusConnection>();
        StatusConnectingConnections = new ConcurrentQueue<StatusConnection>();

        ServerBroadcasts = new ConcurrentQueue<GamePacket>();

        Listener = new TcpListener(IPAddress.Parse(Settings.Default.UserConnectionIP), Settings.Default.UserConnectionPort);
        Listener.Start();
        ListenerBeginAccept();

        Tickets = new ConcurrentDictionary<string, TicketInformation>();
        TicketListener = new UdpClient(new IPEndPoint(IPAddress.Any, Settings.Default.TicketReceivePort));
        TicketBeginReceive();

        if (StatusPortEnabled)
        {
            StatusListener = new TcpListener(IPAddress.Parse(Settings.Default.UserConnectionIP), Settings.Default.StatusPort);
            StatusListener.Start();
            StatusListenerBeginAccept();
        }
    }

    public static void StopService()
    {
        Stopped = true;

        if (Listener != null)
        {
            Listener.Stop();
            Listener.Dispose();
            Listener = null;
        }

        TicketListener?.Close();
        TicketListener = null;

        if (StatusPortEnabled)
        {
            if (StatusListener != null)
            {
                StatusListener.Stop();
                StatusListener.Dispose();
                StatusListener = null;
            }
        }
    }

    public static void Process()
    {
        try
        {
            foreach (var conn in Connections)
            {
                if (conn.Disconnecting || conn.Account != null || !(SEngine.CurrentTime.Subtract(conn.ConnectedTime).TotalSeconds > 30.0))
                    conn.Process();
                else
                    conn.Close(new Exception("Login timed out, disconnected!"));
            }

            while (!DisconnectingConnections.IsEmpty)
            {
                if (DisconnectingConnections.TryDequeue(out var conn))
                    Connections.Remove(conn);
            }

            while (!ConnectingConnections.IsEmpty)
            {
                if (ConnectingConnections.TryDequeue(out var conn))
                    Connections.Add(conn);
            }


            foreach (var conn in StatusConnections)
                conn.Process();

            while (!StatusDisconnectingConnections.IsEmpty)
            {
                if (StatusDisconnectingConnections.TryDequeue(out var conn))
                    StatusConnections.Remove(conn);
            }

            while (!StatusConnectingConnections.IsEmpty)
            {
                if (StatusConnectingConnections.TryDequeue(out var conn))
                    StatusConnections.Add(conn);
            }


            while (!ServerBroadcasts.IsEmpty)
            {
                if (!ServerBroadcasts.TryDequeue(out var p))
                    continue;
                foreach (var conn in Connections)
                {
                    if (conn.Player != null)
                        conn.SendPacket(p);
                }
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText($".\\Log\\Error\\{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.txt", "TargetSite:\r\n" + ex.TargetSite?.ToString() + "\r\nSource:\r\n" + ex.Source + "\r\nMessage:\r\n" + ex.Message + "\r\nStackTrace:\r\n" + ex.StackTrace);
        }
    }

    private static void ListenerBeginAccept()
    {
        if (!Stopped && Listener.Server.IsBound)
            Listener.BeginAcceptTcpClient(Connection, null);
    }

    private static void Connection(IAsyncResult result)
    {
        try
        {
            if (Stopped || !Listener.Server.IsBound) return;

            var client = Listener.EndAcceptTcpClient(result);
            var ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            if (!SystemInfo.Info.IPBans.ContainsKey(ip) || SystemInfo.Info.IPBans[ip] < SEngine.CurrentTime)
                AddConnection(new SConnection(client));
            else
                client.Client.Close();
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog("Asynchronous connection exception: " + ex.ToString());
        }

        while (!Stopped && Connections.Count > Settings.Default.MaxUserConnections)
            Thread.Sleep(1);

        ListenerBeginAccept();
    }

    private static void StatusListenerBeginAccept()
    {
        if (!Stopped && StatusListener.Server.IsBound)
            StatusListener.BeginAcceptTcpClient(StatusConnection, null);
    }

    private static void StatusConnection(IAsyncResult result)
    {
        if (Stopped || !StatusListener.Server.IsBound) return;

        try
        {
            var client = Listener.EndAcceptSocket(result);
            var ip = client.RemoteEndPoint.ToString().Split(':')[0];

            if (!SystemInfo.Info.IPBans.ContainsKey(ip) || SystemInfo.Info.IPBans[ip] < SEngine.CurrentTime)
                AddStatusConnection(new StatusConnection(client));
            else
                client.Close();
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog("Asynchronous connection exception: " + ex.ToString());
        }

        while (StatusConnections.Count >= 5) //Limit status connections
            Thread.Sleep(1);

        StatusListenerBeginAccept();
    }

    private static void TicketBeginReceive()
    {
        if (TicketListener == null || Stopped) return;

        try
        {
            TicketListener.BeginReceive(TicketReceiveData, null);
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog("Ticket asynchronous receive error : " + ex.Message);
        }
    }

    private static void TicketReceiveData(IAsyncResult result)
    {
        if (TicketListener == null || Stopped) return;

        try
        {
            var datagram = TicketListener.EndReceive(result, ref TicketSenderEndPoint);
            if (datagram == null || datagram.Length == 0)
                return;

            ProcessTicket(datagram);
            TicketBeginReceive();
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog("An error occurred while receiving the login ticket. " + ex.Message);
        }
    }

    private static void ProcessTicket(byte[] datagram)
    {
        var str = Encoding.UTF8.GetString(datagram);
        if (string.IsNullOrEmpty(str))
            return;

        var array = str.Split(';');
        if (array.Length >= 4)
        {
            var ticket = array[0];
            var accountName = array[1];

            // TODO: Add more ticket validation..

            if (string.IsNullOrEmpty(ticket) || string.IsNullOrEmpty(accountName))
            {
                SEngine.AddSystemLog("Invalid login ticket. Ticket: " + ticket);
                return;
            }

            Tickets[ticket] = new TicketInformation
            {
                AccountName = accountName,
                PromoCode = array[2],
                ReferrerCode = array[3],
                ValidTime = SEngine.CurrentTime.AddMinutes(5.0)
            };
        }
    }

    public static void OnDisconnect(object sender, Exception e)
    {
        SConnection conn = sender as SConnection;

        string text = "IP: " + conn.IPAddress;
        if (conn.Account != null)
            text += " AccountName: " + conn.Account.AccountName.V;

        if (conn.Player != null)
            text += " UserName: " + conn.Player.Name;

        text += " Message: " + e.Message;
        SEngine.AddSystemLog(text);
    }

    public static void BlockIP(string ip)
    {
        SystemInfo.Info.AddIPBan(ip, SEngine.CurrentTime.AddMinutes(Settings.Default.AbnormalBlockTime));
    }

    public static void SendMessage(PlayerObject player, string message)
    {
        if (player == null || string.IsNullOrEmpty(message))
            return;

        var buffer = ComposeMessage(0, (uint)player.ObjectID, string.Empty, message, 1);

        player.Enqueue(new SystemMessagePacket
        {
            Description = buffer
        });
    }

    public static void SendAnnouncement(string message, bool rolling = false)
    {
        var buffer = ComposeMessage(0, rolling ? 0x90000002u : 0x90000003u, string.Empty, message, 
            rolling ? (byte)2 : (byte)3);

        Broadcast(new SystemMessagePacket
        {
            Description = buffer
        });

        if (Settings.Default.系统窗口发送 == 1)
        {
            SMain.AddSystemLog(message);
        }
    }

    public static byte[] ComposeMessage(uint channel1, uint channel2, string sender, string message, byte type, int level = 0)
    {
        var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(channel1);
        writer.Write(channel2);
        writer.Write((int)type);
        writer.Write(level);
        writer.Write(Encoding.UTF8.GetBytes(message));
        writer.Write((byte)0);
        writer.Write(Encoding.UTF8.GetBytes(sender));
        writer.Write((byte)0);

        return ms.ToArray();
    }

    public static void Broadcast(GamePacket p)
    {
        if (p != null)
            ServerBroadcasts?.Enqueue(p);
    }

    public static void AddConnection(SConnection conn)
    {
        if (conn != null)
            ConnectingConnections.Enqueue(conn);
    }

    public static void RemoveConnection(SConnection conn)
    {
        if (conn != null)
            DisconnectingConnections.Enqueue(conn);
    }

    public static void AddStatusConnection(StatusConnection conn)
    {
        if (conn != null)
            StatusConnectingConnections.Enqueue(conn);
    }

    public static void RemoveStatusConnection(StatusConnection conn)
    {
        if (conn != null)
            StatusDisconnectingConnections.Enqueue(conn);
    }
}
