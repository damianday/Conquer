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

        Listener = new TcpListener(IPAddress.Any, Config.UserConnectionPort);
        Listener.Start();
        Listener.BeginAcceptTcpClient(Connection, null);

        Tickets = new Dictionary<string, TicketInformation>();
        TicketListener = new UdpClient(new IPEndPoint(IPAddress.Any, Config.TicketReceivePort));
        TicketBeginReceive();
    }

    public static void StopService()
    {
        SMain.AddSystemLog("The server network listener is stopped");
        Stopped = true;
        Listener?.Stop();
        Listener = null;

        SMain.AddSystemLog("The server ticket receiver is stopped");
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
                    conn.Disconnect(new Exception("Login timed out, disconnected!"));
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
                    ConnectingConnections?.Enqueue(new SConnection(client));
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

        while (!Stopped && Connections.Count > Config.MaxUserConnections)
            Thread.Sleep(1);

        if (!Stopped)
            Listener.BeginAcceptTcpClient(Connection, null);
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
            var bytes = TicketListener.EndReceive(result, ref TicketSenderEndPoint);
            if (bytes == null || bytes.Length == 0)
            {
                return;
            }

            ProcessTicket(bytes);
            TicketBeginReceive();
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog("An error occurred while receiving the login ticket. " + ex.Message);
        }
    }

    private static void ProcessTicket(byte[] buffer)
    {
        var array = Encoding.UTF8.GetString(buffer).Split(';');
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
        SystemInfo.Info.AddIPBan(ip, SEngine.CurrentTime.AddMinutes(Config.AbnormalBlockTime));
    }

    public static void SendMessage(PlayerObject player, string message)
    {
        SendChatMessage(player, message);
    }

    public enum MsgType
    {
        Normal = 1,
        RollingAnnouncement = 2,
        Announcement = 3,
    }

    private static void SendChatMessage(PlayerObject player, string message)
    {
        if (player == null || string.IsNullOrEmpty(message))
            return;

        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write(0);
        binaryWriter.Write(0);
        binaryWriter.Write(1);
        binaryWriter.Write((int)player.CurrentLevel);
        binaryWriter.Write(Encoding.UTF8.GetBytes(message + "\0"));
        binaryWriter.Write(string.Empty);
        binaryWriter.Write((byte)0);
        player.Enqueue(new SystemMessagePacket
        {
            Description = memoryStream.ToArray()
        });
    }

    public static void SendAnnouncement(string message, bool rolling = false)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(0);
            binaryWriter.Write(rolling ? 2415919106u : 2415919107u);
            binaryWriter.Write(rolling ? 2 : 3);
            binaryWriter.Write(0);
            binaryWriter.Write(Encoding.UTF8.GetBytes(message + "\0"));
            Broadcast(new SystemMessagePacket
            {
                Description = memoryStream.ToArray()
            });
        }
        if (Config.系统窗口发送 == 1)
        {
            SMain.AddSystemLog(message);
        }
    }

    /*public static void SendAnnouncement1(string message, string color, int sendType)
    {
        string format = "<font color='{0}'>{1}</font>";
        format = string.Format(format, color, message);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            byte[] array = null;
            using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(0);
            binaryWriter.Write((byte)3);
            binaryWriter.Write((short)0);
            binaryWriter.Write((byte)144);
            binaryWriter.Write((short)sendType);
            binaryWriter.Write(new byte[6]);
            binaryWriter.Write(Encoding.UTF8.GetBytes(format + "\0"));
            array = memoryStream.ToArray();
            Broadcast(new SystemMessagePacket
            {
                Description = array
            });
        }
        SMain.AddSystemLog(message);
    }*/

    public static void SendMessage(int sender, int target, string uname, string message, int level, MsgType type)
    {
        var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(sender);
        writer.Write(target);
        writer.Write((int)type);
        writer.Write(level);
        writer.Write(Encoding.UTF8.GetBytes(message + "\0"));
        writer.Write(Encoding.UTF8.GetBytes(uname + "\0"));

        Broadcast(new SystemMessagePacket
        {
            Description = ms.ToArray()
        });
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
