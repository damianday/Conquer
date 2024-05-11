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

    public static bool Stopped;

    public static uint ActiveConnections;
    public static uint ConnectionsOnline;
    public static uint ConnectionsOnline1;
    public static uint ConnectionsOnline2;

    public static long TotalSentBytes;
    public static long TotalReceivedBytes;

    public static HashSet<SConnection> Connections;
    public static ConcurrentQueue<SConnection> DisconnectingConnections;
    public static ConcurrentQueue<SConnection> ConnectingConnections;
    public static ConcurrentQueue<GamePacket> ServerAnnouncements;

    public static Dictionary<string, TicketInformation> Tickets;

    public static void StartService()
    {
        Stopped = false;

        Connections = new HashSet<SConnection>();
        ConnectingConnections = new ConcurrentQueue<SConnection>();
        DisconnectingConnections = new ConcurrentQueue<SConnection>();
        ServerAnnouncements = new ConcurrentQueue<GamePacket>();

        Listener = new TcpListener(IPAddress.Parse(Settings.Default.UserConnectionIP), Settings.Default.UserConnectionPort);
        Listener.Start();
        ListenerBeginAccept();

        Tickets = new Dictionary<string, TicketInformation>();
        TicketListener = new UdpClient(new IPEndPoint(IPAddress.Any, Settings.Default.TicketReceivePort));
        TicketBeginReceive();
    }

    public static void StopService()
    {
        Stopped = true;

        Listener?.Stop();
        Listener = null;

        TicketListener?.Close();
        TicketListener = null;
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

            while (!ServerAnnouncements.IsEmpty)
            {
                if (!ServerAnnouncements.TryDequeue(out var p))
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
        if (!Stopped)
            Listener.BeginAcceptTcpClient(Connection, null);
    }

    private static void Connection(IAsyncResult result)
    {
        try
        {
            if (Stopped) return;

            TcpClient client = Listener.EndAcceptTcpClient(result);
            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
            if (!SystemInfo.Info.IPBans.ContainsKey(ip) || SystemInfo.Info.IPBans[ip] < SEngine.CurrentTime)
            {
                if (Connections.Count < 65535)
                    AddConnection(new SConnection(client));
            }
            else
            {
                client.Client.Close();
            }
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog("Asynchronous connection exception: " + ex.ToString());
        }

        while (!Stopped && Connections.Count > Settings.Default.MaxUserConnections)
            Thread.Sleep(1);

        ListenerBeginAccept();
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
        if (array.Length == 4)
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
            ServerAnnouncements?.Enqueue(p);
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
}
