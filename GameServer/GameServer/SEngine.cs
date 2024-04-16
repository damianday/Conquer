using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using GameServer.Map;
using GameServer.Database;
using GameServer.Networking;

namespace GameServer;

public static class SEngine
{
    public static DateTime CurrentTime;
    public static DateTime OneSecondTime;
    public static DateTime NextSaveDataTime;
    public static DateTime 自动保存日志;
    public static DateTime DowntimeTime;

    public static ConcurrentQueue<GMCommand> ExternalCommands;

    public static uint CycleCount;
    public static bool Running;
    public static bool Saving;

    public static Thread MainThread;

    public static Random Random;


    static SEngine()
    {
        CurrentTime = DateTime.Now;
        OneSecondTime = DateTime.Now.AddSeconds(1.0);
        Random = new Random();
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
            if (cmd.Priority == ExecutionPriority.Immediate)
            {
                cmd.ExecuteCommand();
            }
            else if (cmd.Priority == ExecutionPriority.ImmediateBackground)
            {
                if (Running)
                    ExternalCommands.Enqueue(cmd);
                else
                    cmd.ExecuteCommand();
            }
            else if (cmd.Priority == ExecutionPriority.Background)
            {
                if (Running)
                    ExternalCommands.Enqueue(cmd);
                else
                    SMain.AddCommandLog("<= Command execution failed, the current command can only be executed when the server is running, please start the server first.");
            }
            else if (cmd.Priority == ExecutionPriority.Inactive)
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
            ExternalCommands = new ConcurrentQueue<GMCommand>();
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

                CurrentTime = DateTime.Now;
                if (CurrentTime > OneSecondTime)
                {
                    ProcessSaveData();

                    SMain.更新连接总数((uint)NetworkManager.Connections.Count);
                    SMain.更新已经登录(NetworkManager.ActiveConnections);
                    SMain.更新已经上线(NetworkManager.已上线连接数, NetworkManager.已上线连接数1, NetworkManager.已上线连接数2);
                    SMain.更新发送字节(NetworkManager.TotalSentBytes);
                    SMain.更新接收字节(NetworkManager.TotalReceivedBytes);
                    SMain.更新对象统计(MapManager.ActiveObjects.Count, MapManager.SecondaryObjects.Count, MapManager.Objects.Count);
                    SMain.UpdateCycleCount(CycleCount);

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
            SMain.AddSystemLog("正在清理物品数据...");
            MapManager.RemoveItems();
            SMain.AddSystemLog("正在保存客户数据...");
            Session.Save();
            Session.SaveUsers();
            SMain.AddSystemLog("服务器停止使用");
            SMain.OnStopServiceCompleted();
            SMain.AddSystemLog("服务器线程关闭");
            MainThread = null;
            SMain.AddSystemLog("服务器已成功关闭");
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
