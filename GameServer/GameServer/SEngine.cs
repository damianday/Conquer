using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using GameServer.Map;
using GameServer.Database;
using GameServer.Networking;

namespace GameServer;


public sealed class SEngineStats
{
    public uint Connections;
    public uint ActiveConnections;
    public uint ConnectionsOnline, ConnectionsOnline1, ConnectionsOnline2;
    public long TotalSentBytes;
    public long TotalReceivedBytes;
    public int ActiveObjects, SecondaryObjects, Objects;
    public uint CycleCount;
}

public static class SEngine
{
    public static DateTime CurrentTime;
    public static DateTime OneSecondTime;
    public static DateTime NextSaveDataTime;
    public static DateTime 自动保存日志;
    public static DateTime DowntimeTime;

    public static ConcurrentQueue<GMCommand> ExternalCommands;

    public static SEngineStats Stats;
    public static uint CycleCount;
    public static bool Running;
    public static bool Saving;

    public static Thread MainThread;

    public static Random Random;
    public static MsgFilter Abuse;


    static SEngine()
    {
        CurrentTime = DateTime.UtcNow;
        OneSecondTime = CurrentTime.AddSeconds(1.0);
        NextSaveDataTime = CurrentTime.AddMinutes(Config.AutoSaveInterval);

        ExternalCommands = new ConcurrentQueue<GMCommand>();

        Stats = new SEngineStats();
        CycleCount = 0;
        Running = false;
        Saving = false;

        MainThread = null;

        Random = new Random();
        Abuse = new MsgFilter();
    }

    public static void StartService()
    {
        if (!Running)
        {
            (MainThread = new Thread(ServiceThreadLoop)
            {
                IsBackground = true
            }).Start();
        }
    }

    public static void StopService()
    {
        SMain.AddSystemLog("Server shutting down..");
        Running = false;
        SMain.AddSystemLog("Server stopped.");
        NetworkManager.StopService();
    }

    public static void AddSystemLog(string message)
    {
        SMain.AddSystemLog(message);
    }

    public static void AddChatLog(string tag, byte[] message)
    {
        SMain.AddChatLog(tag, message);
    }

    public static bool AddGMCommand(string cmdText)
    {
        if (string.IsNullOrEmpty(cmdText))
            return false;

        if (!cmdText.StartsWith('@'))
        {
            SMain.AddCommandLog("<= Command parsing error, GM commands must start with '@'. Type '@ViewCommands' to get all supported command formats.");
            return false;
        }

        if (cmdText.Trim('@', ' ').Length == 0)
        {
            SMain.AddCommandLog("<= Command parsing error, GM command cannot be null. Type '@ViewCommands' to get all supported command formats.");
            return false;
        }

        if (GMCommand.ParseCommand(cmdText, out var cmd))
        {
            if (cmd.Priority == ExecuteCondition.Immediate)
            {
                cmd.ExecuteCommand();
            }
            else if (cmd.Priority == ExecuteCondition.Normal)
            {
                if (Running)
                    ExternalCommands.Enqueue(cmd);
                else
                    cmd.ExecuteCommand();
            }
            else if (cmd.Priority == ExecuteCondition.Background)
            {
                if (Running)
                    ExternalCommands.Enqueue(cmd);
                else
                    SMain.AddCommandLog("<= Command execution failed, the current command can only be executed when the server is running, please start the server first.");
            }
            else if (cmd.Priority == ExecuteCondition.Inactive)
            {
                if (!Running && (MainThread == null || !MainThread.IsAlive))
                    cmd.ExecuteCommand();
                else
                    SMain.AddCommandLog("<= Command execution failed, the current command can only be executed when the server is not running, please shut down the server first.");
            }
            return true;
        }

        return false;
    }

    private static void ServiceThreadLoop()
    {
        try
        {
            ExternalCommands.Clear();

            SMain.AddSystemLog("Loading Abuse...");
            if (Abuse.Load("!Abuse.txt"))
                SMain.AddSystemLog("!Abuse.txt loaded..");

            SMain.AddSystemLog("Loading maps...");
            MapManager.Initialize();
            SMain.AddSystemLog("The network service is being started...");
            NetworkManager.StartService();
            SMain.AddSystemLog("Server successfully started.");
            Running = true;
            SMain.OnStartServiceCompleted();
            while (Running || NetworkManager.Connections.Count > 0)
            {
                Thread.Sleep(1);

                CurrentTime = DateTime.UtcNow;
                if (CurrentTime > OneSecondTime)
                {
                    ProcessSaveData();

                    Stats.Connections = (uint)NetworkManager.Connections.Count;
                    Stats.ActiveConnections = NetworkManager.ActiveConnections;
                    Stats.ConnectionsOnline = NetworkManager.ConnectionsOnline;
                    Stats.ConnectionsOnline = NetworkManager.ConnectionsOnline1;
                    Stats.ConnectionsOnline = NetworkManager.ConnectionsOnline2;
                    Stats.TotalSentBytes = NetworkManager.TotalReceivedBytes;
                    Stats.TotalReceivedBytes = NetworkManager.TotalReceivedBytes;
                    Stats.ActiveObjects = MapManager.ActiveObjects.Count;
                    Stats.SecondaryObjects = MapManager.SecondaryObjects.Count;
                    Stats.Objects = MapManager.Objects.Count;
                    Stats.CycleCount = CycleCount;
                    SMain.UpdateStats(Stats);

                    CycleCount = 0;
                    OneSecondTime = CurrentTime.AddSeconds(1.0);
                }
                else
                {
                    CycleCount++;
                }

                while (ExternalCommands.TryDequeue(out var cmd))
                    cmd.ExecuteCommand();

                NetworkManager.Process();
                MapManager.Process();
            }
            SMain.AddSystemLog("Cleaning item data...");
            MapManager.RemoveItems();
            SMain.AddSystemLog("Saving customer data...");
            Session.Save();
            Session.SaveUsers();
            SMain.AddSystemLog("Server is out of service");
            SMain.OnStopServiceCompleted();
            SMain.AddSystemLog("Server thread shut down");
            MainThread = null;
            SMain.AddSystemLog("Server has been shut down successfully");
        }
        catch (Exception ex)
        {
            if (CurrentTime > DowntimeTime)
            {
                if (!Directory.Exists(".\\Log\\Error"))
                    Directory.CreateDirectory(".\\Log\\Error");

                File.WriteAllText($".\\Log\\Error\\{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.txt", "TargetSite:\r\n" + ex.TargetSite?.ToString() + "\r\nHelpLink:\r\n" + ex.HelpLink + "\r\nInnerException:\r\n" + ex.InnerException?.ToString() + "\r\nSource:\r\n" + ex.Source + "\r\nMessage:\r\n" + ex.Message + "\r\nStackTrace:\r\n" + ex.StackTrace);
                SMain.AddSystemLog("An error has occured, please check the log files");
                DowntimeTime = CurrentTime.AddSeconds(60.0);
            }
        }
    }

    private static void ProcessSaveData()
    {
        if (CurrentTime > NextSaveDataTime)
        {
            Session.AutoSave();
            Session.SaveUsers();
            SMain.AddSystemLog("The automatic storage of data has been completed");

            NextSaveDataTime = CurrentTime.AddMinutes(Config.AutoSaveInterval);
        }
        if (自动保存日志 > CurrentTime)
        {
            自动保存日志 = CurrentTime.AddMinutes(Config.自动保存日志);
        }
    }
}
