using System;
using System.Reflection;
using System.Text;
using System.Net.Sockets;

namespace GameServer.Networking;

public sealed class StatusConnection
{
    public readonly string IPAddress;
    private Socket Connection;

    private DateTime NextSendTime;

    private bool _disconnecting;
    public bool Connected;
    public bool Disconnecting
    {
        get { return _disconnecting; }
        set
        {
            if (_disconnecting == value) return;
            _disconnecting = value;
            DisconnectTime = SEngine.CurrentTime.AddMilliseconds(500);
        }
    }
    public readonly DateTime ConnectedTime;
    public DateTime DisconnectTime;


    public StatusConnection(Socket client)
    {
        try
        {
            IPAddress = client.RemoteEndPoint.ToString().Split(':')[0];

            Connection = client;
            Connection.NoDelay = true;

            ConnectedTime = SEngine.CurrentTime;
            DisconnectTime = SEngine.CurrentTime.AddSeconds(Settings.Default.StatusPortDisconnectTime);
            Connected = true;
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog(ex.ToString());
        }
    }

    private void BeginSend(byte[] data)
    {
        if (!Connected || data.Length == 0) return;

        try
        {
            Connection.BeginSend(data, 0, data.Length, SocketFlags.None, SendData, null);
        }
        catch
        {
            Disconnecting = true;
        }
    }

    private void SendData(IAsyncResult result)
    {
        try
        {
            Connection.EndSend(result);
        }
        catch
        { }
    }

    public void Process()
    {
        try
        {
            if (Connection == null || !Connection.Connected)
            {
                Disconnect();
                return;
            }

            if (SEngine.CurrentTime > DisconnectTime || Disconnecting)
            {
                Disconnect();
                return;
            }

            if (SEngine.CurrentTime > NextSendTime)
            {
                NextSendTime = SEngine.CurrentTime.AddSeconds(10);

                var version = Assembly.GetCallingAssembly().GetName().Version;
                var serverName = "Name";
                string output = $"c;/{serverName}/{NetworkManager.ActiveConnections}/LegendsEternal/{version}//;";

                BeginSend(Encoding.ASCII.GetBytes(output));
            }
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog(ex.ToString());
        }
    }

    public void Disconnect()
    {
        try
        {
            if (!Connected) return;

            Connected = false;

            NetworkManager.RemoveStatusConnection(this);

            Connection?.Shutdown(SocketShutdown.Both);
            Connection?.Close();
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog(ex.ToString());
        }
    }

    public void Close()
    {
        if (!Disconnecting)
        {
            Disconnecting = true;
        }
    }
}
