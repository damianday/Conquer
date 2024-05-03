using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccountServer.Networking;

public static class SEngine
{
	public static DateTime CurrentTime;
	private static DateTime StatsTime;

	private static TcpListener Listener;
	private static UdpClient TicketSender;

	public static bool Running { get; set; }

	public static long TotalBytesSent;
	public static long TotalBytesReceived;

	private static HashSet<SConnection> Connections = new HashSet<SConnection>();
	private static ConcurrentQueue<SConnection> RemovingConnections = new ConcurrentQueue<SConnection>();
	private static ConcurrentQueue<SConnection> AddingConnections = new ConcurrentQueue<SConnection>();

	public static void StartService()
	{
		if (Running) return;

		Connections.Clear();
		RemovingConnections.Clear();
		AddingConnections.Clear();

		try
		{
			Listener = new TcpListener(IPAddress.Parse(Settings.Default.LocalListeningIP), Settings.Default.LocalListeningPort);
			Listener.Start();
			Listener.BeginAcceptTcpClient(Connection, null);

			TicketSender = new UdpClient();

            Running = true;
            Task.Run(delegate
			{
				while (Running)
				{
					Process();

					Thread.Sleep(1);
				}
			});
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage(ex.Message);

			Listener?.Stop();
			Listener = null;

			TicketSender?.Close();
			TicketSender = null;
		}
	}

	public static void StopService()
	{
		if (!Running) return;

		Running = false;

		Listener?.Stop();
		Listener = null;

		TicketSender?.Close();
		TicketSender = null;
	}

	public static void Process()
	{
		CurrentTime = DateTime.Now;

		foreach (var conn in Connections)
			conn.Process();

		while (!RemovingConnections.IsEmpty)
		{
			if (RemovingConnections.TryDequeue(out var conn))
				Connections.Remove(conn);
		}
		while (!AddingConnections.IsEmpty)
		{
			if (AddingConnections.TryDequeue(out var conn))
				Connections.Add(conn);
		}

		if (CurrentTime > StatsTime)
		{
			SMain.UpdateServerStats();
			StatsTime = CurrentTime.AddSeconds(1);
		}
	}

	public static void Connection(IAsyncResult result)
	{
		try
		{
			if (!Running) return;

			var client = Listener.EndAcceptTcpClient(result);
			AddingConnections.Enqueue(new SConnection(client));
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Asynchronous connection exception: " + ex.ToString());
		}

		if (Running)
			Listener.BeginAcceptTcpClient(Connection, null);
	}

	public static void AddConnection(SConnection conn)
	{
		if (conn != null)
			AddingConnections.Enqueue(conn);
	}

	public static void RemoveConnection(SConnection conn)
	{
		if (conn != null)
			RemovingConnections.Enqueue(conn);
	}

	public static bool SendData(IPEndPoint address, byte[] datagram)
	{
		if (TicketSender == null || datagram == null) return false;

		try
		{
			var ar = TicketSender.BeginSend(datagram, datagram.Length, address, SendComplete, null);
			return true;
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Data was sent incorrectly: " + ex.Message);
		}
		return false;
	}

	private static void SendComplete(IAsyncResult result)
	{
		try
		{
			var dataSent = TicketSender.EndSend(result);
			if (dataSent == 0)
			{
				SMain.AddLogMessage("Sending Callback Error!");
			}
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Sending Callback Error: " + ex.Message);
		}
	}

	public static void SendTicketToServer(IPEndPoint address, string ticket, string account, string promoCode, string referralCode)
	{
		try
		{
			var data = Encoding.UTF8.GetBytes(ticket + ";" + account + ";" + promoCode + ";" + referralCode);
			if (SendData(address, data))
				SMain.TotalTickets++;
			else
				SMain.AddLogMessage("Ticket delivery failed.");
		}
		catch (Exception ex)
		{
			SMain.AddLogMessage("Failed to send tickets: " + ex.Message);
		}
	}
}
