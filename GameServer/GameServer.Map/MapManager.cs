using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using GameServer.Database;
using GameServer.Template;
using GameServer.Networking;

using GamePackets.Server;

namespace GameServer.Map;

public static class MapManager
{
    public static int 对象表计数;

    public static List<MapObject> SecondaryObjects;
    public static List<MapObject> BackupObjects;
    public static Dictionary<int, MapObject> ActiveObjects;
    public static Dictionary<int, MapObject> Objects;
    public static Dictionary<int, PlayerObject> Players;
    public static Dictionary<int, PetObject> Pets;
    public static Dictionary<int, MonsterObject> Monsters;
    public static Dictionary<int, GuardObject> Guards;
    public static Dictionary<int, ItemObject> Items;
    public static Dictionary<int, TrapObject> Traps;
    public static Dictionary<int, Map> Maps;

    public static HashSet<Map> ReplicateMaps;

    private static ConcurrentQueue<Map> ReplicateClosingMaps;
    private static ConcurrentQueue<MapObject> ActivationQueue;
    private static ConcurrentQueue<MapObject> DeactivationQueue;

    public static int ObjectID;
    public static int TrapObjectID;
    public static int ItemObjectID;

    public static DateTime CurrentTime;
    private static DateTime SandCityTime;
    private static DateTime SandCityTime1;

    public static Point 沙城城门坐标;
    public static Point 皇宫下门坐标;
    public static Point 皇宫下门出口;
    public static Point 皇宫下门入口;
    public static Point 皇宫左门坐标;
    public static Point 皇宫左门出口;
    public static Point 皇宫左门入口;
    public static Point 皇宫上门坐标;
    public static Point 皇宫上门出口;
    public static Point 皇宫上门入口;
    public static Point 皇宫出口点一;
    public static Point 皇宫出口点二;
    public static Point 皇宫正门入口;
    public static Point 皇宫正门出口;
    public static Point 皇宫入口点左;
    public static Point 皇宫入口点中;
    public static Point 皇宫入口点右;
    public static Point 八卦坛坐标上;
    public static Point 八卦坛坐标下;
    public static Point 八卦坛坐标左;
    public static Point 八卦坛坐标右;
    public static Point 八卦坛坐标中;

    public static MonsterObject 魔火龙;
    public static Map 世界BOSS地图;

    public static MonsterObject BOSS名字二;
    public static Map BOSS二地图编号;

    public static MonsterObject BOSS名字三;
    public static Map BOSS三地图编号;

    public static MonsterObject BOSS名字四;
    public static Map BOSS四地图编号;

    public static MonsterObject BOSS名字五;
    public static Map BOSS五地图编号;

    public static MonsterObject BOSS名字一;
    public static Map BOSS一地图编号;
    public static Map 沙城地图;

    public static MonsterObject 沙城城门;
    public static MonsterObject 下方宫门;
    public static MonsterObject 上方宫门;
    public static MonsterObject 左方宫门;

    public static GuardObject 上方法阵;
    public static GuardObject 下方法阵;
    public static GuardObject 左方法阵;
    public static GuardObject 右方法阵;
    public static GuardObject 八卦坛激活法阵;

    public static GuildInfo 八卦坛激活行会;

    public static DateTime 八卦坛激活计时;

    public static MapArea 皇宫随机区域;
    public static MapArea 外城复活区域;
    public static MapArea 内城复活区域;
    public static MapArea 守方传送区域;
    public static MapArea 攻沙快捷;
    public static MapArea 传送区域沙左;
    public static MapArea 传送区域沙右;
    public static MapArea 传送区域皇宫;

    public static byte SandCityStage;

    public static DateTime 通知时间;
    public static DateTime 通知时间2;
    public static DateTime 通知时间3;
    public static DateTime 通知时间4;
    public static DateTime 通知时间5;
    public static DateTime 通知时间6;
    public static DateTime 通知时间7;

    public static HashSet<GuildInfo> SiegeGuilds;

    private static void ProcessSandCity()
    {
        var dayOfWeek = DateTime.Now.DayOfWeek;

        if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && Config.沙巴克每周攻沙时间 == 0)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek == DayOfWeek.Monday && Config.沙巴克每周攻沙时间 == 1)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek == DayOfWeek.Tuesday && Config.沙巴克每周攻沙时间 == 2)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek == DayOfWeek.Wednesday && Config.沙巴克每周攻沙时间 == 3)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek == DayOfWeek.Thursday && Config.沙巴克每周攻沙时间 == 4)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek == DayOfWeek.Friday && Config.沙巴克每周攻沙时间 == 5)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek == DayOfWeek.Saturday && Config.沙巴克每周攻沙时间 == 6)
        {
            ProcessSabakWar();
        }
        else
        {
            if (SEngine.CurrentTime.Hour + 2 < Config.沙巴克开启 || dayOfWeek != DayOfWeek.Sunday || Config.沙巴克每周攻沙时间 != 7)
                return;

            ProcessSabakWar();
        }
    }

    private static void ProcessSabakWar()
    {
        static GuildInfo FindGuild(Map map)
        {
            bool flag = true;
            GuildInfo guild = null;
            foreach (var point in 皇宫随机区域.RangeCoordinates)
            {
                foreach (var obj in map[point])
                {
                    if (!obj.Dead && obj is PlayerObject player)
                    {
                        if (player.Guild == null || !SiegeGuilds.Contains(player.Guild))
                        {
                            flag = false;
                            break;
                        }
                        if (guild == null)
                        {
                            guild = player.Guild;
                        }
                        else if (guild != player.Guild)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (!flag) break;
            }
            return guild;
        }

        var map = GetMap(152);

        foreach (MapObject obj in map[皇宫下门坐标].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫下门入口);
            }
        }
        foreach (MapObject obj in map[皇宫上门坐标].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫上门入口);
            }
        }
        foreach (MapObject obj in map[皇宫左门坐标].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫左门入口);
            }
        }
        foreach (MapObject obj in map[皇宫出口点一].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫正门出口);
            }
        }
        foreach (MapObject obj in map[皇宫出口点二].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫正门出口);
            }
        }
        foreach (MapObject obj in map[皇宫入口点左].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime && player.Guild != null && player.Guild == SystemInfo.Info.OccupyGuild.V)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫正门入口);
            }
        }
        foreach (MapObject obj in map[皇宫入口点中].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime && player.Guild != null && player.Guild == SystemInfo.Info.OccupyGuild.V)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫正门入口);
            }
        }
        foreach (MapObject obj in map[皇宫入口点右].ToList())
        {
            if (!obj.Dead && obj is PlayerObject player && SEngine.CurrentTime > player.BusyTime && player.Guild != null && player.Guild == SystemInfo.Info.OccupyGuild.V)
            {
                player.Teleport(map, AreaType.Unknown, 皇宫正门入口);
            }
        }

        if (SEngine.CurrentTime < SandCityTime) return;

        if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
        {
            SandCityStage = 0;
        }

        if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && SandCityStage == 0)
        {
            NetworkManager.SendAnnouncement("The Siege of Sabak will begin in ten minutes, so be prepared", true);
            SandCityStage = 1;
        }

        if (Config.沙巴克停止开关 == 1)
        {
            SandCityStage = 0;
            Config.沙巴克停止开关 = 0;

            GuildInfo guild = FindGuild(map);
 
            if (SiegeGuilds.Contains(guild) && guild != null)
            {
                SystemInfo.Info.OccupyGuild.V = guild;
                SystemInfo.Info.SabakOccupyTime.V = SEngine.CurrentTime;
                foreach (var member in guild.Members)
                {
                    member.Key.攻沙日期.V = SEngine.CurrentTime;
                }
                NetworkManager.SendAnnouncement($"The siege of Sabak is over, [{guild}] Became the new Sabak Guild", true);
                SandCityStage = 6;
                NetworkManager.Broadcast(new 同步占领行会
                {
                    NewGuildID = guild.ID
                });
            }
        }

        if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
        {
            SystemInfo.Info.OccupyGuild.V = null;
            Config.沙巴克重置系统 = 0;
            NetworkManager.SendAnnouncement("Sabak reset, about to start the siege, please get ready", true);
            SandCityStage = 1;
        }

        if (SandCityStage == 1)
        {
            SandCityStage = 2;
            MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value);
            MonsterObject 怪物实例2 = new MonsterObject(value, map, int.MaxValue, new Point[1] { 沙城城门坐标 }, true, true);
            怪物实例2.CurrentDirection = GameDirection.UpRight;
            怪物实例2.SurvivalTime = DateTime.MaxValue;
            沙城城门 = 怪物实例2;
            MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value2);
            怪物实例2 = new MonsterObject(value2, map, int.MaxValue, new Point[1] { 皇宫上门坐标 }, true, true);
            怪物实例2.CurrentDirection = GameDirection.DownRight;
            怪物实例2.SurvivalTime = DateTime.MaxValue;
            上方宫门 = 怪物实例2;
            怪物实例2 = new MonsterObject(value2, map, int.MaxValue, new Point[1] { 皇宫下门坐标 }, true, true);
            怪物实例2.CurrentDirection = GameDirection.DownRight;
            怪物实例2.SurvivalTime = DateTime.MaxValue;
            下方宫门 = 怪物实例2;
            怪物实例2 = new MonsterObject(value2, map, int.MaxValue, new Point[1] { 皇宫左门坐标 }, true, true);
            怪物实例2.CurrentDirection = GameDirection.DownLeft;
            怪物实例2.SurvivalTime = DateTime.MaxValue;
            左方宫门 = 怪物实例2;
            GameBuff.DataSheet.TryGetValue(22300, out var value3);
            沙城城门.AddBuff(value3.ID, 沙城城门);
            上方宫门.AddBuff(value3.ID, 上方宫门);
            下方宫门.AddBuff(value3.ID, 下方宫门);
            左方宫门.AddBuff(value3.ID, 左方宫门);
        }

        if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && SandCityStage == 2)
        {
            if (SystemInfo.Info.OccupyGuild.V != null)
            {
                SiegeGuilds.Add(SystemInfo.Info.OccupyGuild.V);
                NetworkManager.SendAnnouncement($"Gain the qualification guild to capture Sabak【{SystemInfo.Info.OccupyGuild.V}】", true);
            }
            foreach (KeyValuePair<DateTime, GuildInfo> item12 in SystemInfo.Info.申请行会.ToList())
            {
                SiegeGuilds.Add(SystemInfo.Info.OccupyGuild.V);
                SiegeGuilds.Add(item12.Value);
                SystemInfo.Info.申请行会.Remove(item12.Key);
                NetworkManager.SendAnnouncement($"Gain the qualification guild to capture Sabak【{item12.Value}】", true);
            }
            if (SiegeGuilds.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
            {
                SandCityStage = 0;
                沙城城门?.Despawn();
                上方宫门?.Despawn();
                下方宫门?.Despawn();
                左方宫门?.Despawn();
                NetworkManager.SendAnnouncement($"Only the Sabak Guild is registered! The Sabak Siege is closed and the Guild is occupied【{SystemInfo.Info.OccupyGuild.V}】", true);
                return;
            }
            if (SiegeGuilds.Count == 0)
            {
                SandCityStage = 0;
                沙城城门?.Despawn();
                上方宫门?.Despawn();
                下方宫门?.Despawn();
                左方宫门?.Despawn();
                NetworkManager.SendAnnouncement("There are no siege guilds today, and the Sabak siege is closed!", true);
                return;
            }
            SandCityStage = 3;
        }

        if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && SandCityStage == 3)
        {
            沙城城门.移除Buff时处理(22300);
            下方宫门.移除Buff时处理(22300);
            上方宫门.移除Buff时处理(22300);
            左方宫门.移除Buff时处理(22300);
            NetworkManager.SendAnnouncement("The Siege of Sabak begins! Warriors fight for honor!", true);
            SandCityStage = 4;
        }

        if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && SandCityStage == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
        {
            NetworkManager.SendAnnouncement("The gates of Sabak have been breached", true);
            沙城城门.BirthMap = null;
            SandCityStage = 5;
        }

        if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && SandCityStage == 5)
        {
            皇宫随机区域 = map.Areas.FirstOrDefault(x => x.RegionName == "沙巴克-皇宫随机区域");

            GuildInfo guild = FindGuild(map);

            if (SiegeGuilds.Contains(guild) && SEngine.CurrentTime >= SandCityTime1 && guild != null)
            {
                SystemInfo.Info.OccupyGuild.V = guild;
                SystemInfo.Info.SabakOccupyTime.V = SEngine.CurrentTime;
                NetworkManager.Broadcast(new 同步占领行会
                {
                    NewGuildID = guild.ID
                });
                SandCityTime1 = SEngine.CurrentTime.AddSeconds(30.0);
            }
        }

        if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && SandCityStage == 5)
        {
            GuildInfo guild = FindGuild(map);

            if (SiegeGuilds.Contains(guild) && guild != null)
            {
                SystemInfo.Info.OccupyGuild.V = guild;
                SystemInfo.Info.SabakOccupyTime.V = SEngine.CurrentTime;
                foreach (var member in guild.Members)
                {
                    member.Key.攻沙日期.V = SEngine.CurrentTime;
                }
                NetworkManager.SendAnnouncement($"The siege of Sabak is over, [{guild}] Became the new Sabak Guild", true);
                SandCityStage = 6;
                NetworkManager.Broadcast(new 同步占领行会
                {
                    NewGuildID = guild.ID
                });
            }
        }

        SandCityTime = SEngine.CurrentTime.AddMilliseconds(1000.0);
    }

    public static void Process()
    {
        try
        {
            foreach (var obj in ActiveObjects)
                obj.Value?.Process();

            if (对象表计数 >= SecondaryObjects.Count)
            {
                对象表计数 = 0;
                SecondaryObjects = BackupObjects;
                BackupObjects = new List<MapObject>();
            }
            for (int i = 0; i < 100; i++)
            {
                if (对象表计数 >= SecondaryObjects.Count)
                {
                    break;
                }
                if (SecondaryObjects[对象表计数].SecondaryObject)
                {
                    SecondaryObjects[对象表计数].Process();
                    BackupObjects.Add(SecondaryObjects[对象表计数]);
                }
                对象表计数++;
            }
            while (!DeactivationQueue.IsEmpty)
            {
                if (DeactivationQueue.TryDequeue(out var result) && !result.Activated)
                {
                    ActiveObjects.Remove(result.ObjectID);
                }
            }
            while (!ActivationQueue.IsEmpty)
            {
                if (ActivationQueue.TryDequeue(out var result2) && result2.Activated && !ActiveObjects.ContainsKey(result2.ObjectID))
                {
                    ActiveObjects.Add(result2.ObjectID, result2);
                }
            }
            if (SEngine.CurrentTime.Minute == 55 && SEngine.CurrentTime.Hour != 通知时间.Hour)
            {
                if (SEngine.CurrentTime.Hour + 1 == Config.武斗场时间一 || SEngine.CurrentTime.Hour + 1 == Config.武斗场时间二)
                {
                    NetworkManager.SendAnnouncement("The Experience Arena will open in five minutes, so be prepared if you want to participate", rolling: true);
                }
                通知时间 = SEngine.CurrentTime;
            }
            if (Config.CurrentVersion >= 3 && SEngine.CurrentTime.Minute == Config.世界BOSS分钟 && SEngine.CurrentTime.Hour != 通知时间3.Hour && SEngine.CurrentTime.Second == 1)
            {
                if (SEngine.CurrentTime.Hour == Config.世界BOSS时间 && MonsterInfo.DataSheet.TryGetValue(Config.世界BOSS名字, out var value))
                {
                    Map 出生地图 = GetMap(74);
                    魔火龙 = new MonsterObject(value, 出生地图, int.MaxValue, new Point[1]
                    {
                        new Point(1043, 176)
                    }, forbidResurrection: true, 立即刷新: true)
                    {
                        CurrentDirection = GameDirection.UpRight,
                        SurvivalTime = DateTime.MaxValue
                    };
                    NetworkManager.SendAnnouncement("世界BOSS[ " + Config.世界BOSS名字 + " ]已经降临秘宝广场, 想要参加的勇士请做好准备", rolling: true);
                }
                通知时间3 = SEngine.CurrentTime;
            }
            if (Config.CurrentVersion >= 1 && SEngine.CurrentTime.Minute == Config.BOSS一分钟 && SEngine.CurrentTime.Hour != 通知时间2.Hour && SEngine.CurrentTime.Second == 1 && Config.自动BOSS1界面1开关 == 1)
            {
                if (SEngine.CurrentTime.Hour == Config.BOSS一时间 && MonsterInfo.DataSheet.TryGetValue(Config.BOSS名字一, out var value2))
                {
                    Map 出生地图2 = GetMap(Config.BOSS一地图编号);
                    BOSS名字一 = new MonsterObject(value2, 出生地图2, int.MaxValue, new Point[1]
                    {
                        new Point(Config.BOSS一坐标X, Config.BOSS一坐标Y)
                    }, forbidResurrection: true, 立即刷新: true)
                    {
                        CurrentDirection = GameDirection.UpRight,
                        SurvivalTime = DateTime.MaxValue
                    };
                    NetworkManager.SendAnnouncement("世界BOSS[ " + Config.BOSS名字一 + " ]已经降临" + Config.BOSS一地图名字 + ", 想要参加的勇士请做好准备", rolling: true);
                }
                通知时间2 = SEngine.CurrentTime;
            }
            if (Config.CurrentVersion >= 1 && SEngine.CurrentTime.Minute == Config.BOSS二分钟 && SEngine.CurrentTime.Hour != 通知时间4.Hour && SEngine.CurrentTime.Second == 1 && Config.自动BOSS1界面2开关 == 1)
            {
                if (SEngine.CurrentTime.Hour == Config.BOSS二时间 && MonsterInfo.DataSheet.TryGetValue(Config.BOSS名字二, out var value3))
                {
                    Map 出生地图3 = GetMap(Config.BOSS二地图编号);
                    BOSS名字二 = new MonsterObject(value3, 出生地图3, int.MaxValue, new Point[1]
                    {
                        new Point(Config.BOSS二坐标X, Config.BOSS二坐标Y)
                    }, forbidResurrection: true, 立即刷新: true)
                    {
                        CurrentDirection = GameDirection.UpRight,
                        SurvivalTime = DateTime.MaxValue
                    };
                    NetworkManager.SendAnnouncement("世界BOSS[ " + Config.BOSS名字二 + " ]已经降临" + Config.BOSS二地图名字 + ", 想要参加的勇士请做好准备", rolling: true);
                }
                通知时间4 = SEngine.CurrentTime;
            }
            if (Config.CurrentVersion >= 1 && SEngine.CurrentTime.Minute == Config.BOSS三分钟 && SEngine.CurrentTime.Hour != 通知时间5.Hour && SEngine.CurrentTime.Second == 1 && Config.自动BOSS1界面3开关 == 1)
            {
                if (SEngine.CurrentTime.Hour == Config.BOSS三时间 && MonsterInfo.DataSheet.TryGetValue(Config.BOSS名字三, out var value4))
                {
                    Map 出生地图4 = GetMap(Config.BOSS三地图编号);
                    BOSS名字三 = new MonsterObject(value4, 出生地图4, int.MaxValue, new Point[1]
                    {
                        new Point(Config.BOSS三坐标X, Config.BOSS三坐标Y)
                    }, forbidResurrection: true, 立即刷新: true)
                    {
                        CurrentDirection = GameDirection.UpRight,
                        SurvivalTime = DateTime.MaxValue
                    };
                    NetworkManager.SendAnnouncement("世界BOSS[ " + Config.BOSS名字三 + " ]已经降临" + Config.BOSS三地图名字 + ", 想要参加的勇士请做好准备", rolling: true);
                }
                通知时间5 = SEngine.CurrentTime;
            }
            if (Config.CurrentVersion >= 1 && SEngine.CurrentTime.Minute == Config.BOSS四分钟 && SEngine.CurrentTime.Hour != 通知时间6.Hour && SEngine.CurrentTime.Second == 1 && Config.自动BOSS1界面4开关 == 1)
            {
                if (SEngine.CurrentTime.Hour == Config.BOSS四时间 && MonsterInfo.DataSheet.TryGetValue(Config.BOSS名字四, out var value5))
                {
                    Map 出生地图5 = GetMap(Config.BOSS四地图编号);
                    BOSS名字四 = new MonsterObject(value5, 出生地图5, int.MaxValue, new Point[1]
                    {
                        new Point(Config.BOSS四坐标X, Config.BOSS四坐标Y)
                    }, forbidResurrection: true, 立即刷新: true)
                    {
                        CurrentDirection = GameDirection.UpRight,
                        SurvivalTime = DateTime.MaxValue
                    };
                    NetworkManager.SendAnnouncement("世界BOSS[ " + Config.BOSS名字四 + " ]已经降临" + Config.BOSS四地图名字 + ", 想要参加的勇士请做好准备", rolling: true);
                }
                通知时间6 = SEngine.CurrentTime;
            }
            if (Config.CurrentVersion >= 1 && SEngine.CurrentTime.Minute == Config.BOSS五分钟 && SEngine.CurrentTime.Hour != 通知时间7.Hour && SEngine.CurrentTime.Second == 1 && Config.自动BOSS1界面5开关 == 1)
            {
                if (SEngine.CurrentTime.Hour == Config.BOSS五时间 && MonsterInfo.DataSheet.TryGetValue(Config.BOSS名字五, out var value6))
                {
                    Map 出生地图6 = GetMap(Config.BOSS五地图编号);
                    BOSS名字五 = new MonsterObject(value6, 出生地图6, int.MaxValue, new Point[1]
                    {
                        new Point(Config.BOSS五坐标X, Config.BOSS五坐标Y)
                    }, forbidResurrection: true, 立即刷新: true)
                    {
                        CurrentDirection = GameDirection.UpRight,
                        SurvivalTime = DateTime.MaxValue
                    };
                    NetworkManager.SendAnnouncement("世界BOSS[ " + Config.BOSS名字五 + " ]已经降临" + Config.BOSS五地图名字 + ", 想要参加的勇士请做好准备", rolling: true);
                }
                通知时间7 = SEngine.CurrentTime;
            }
            foreach (Map map in ReplicateMaps)
            {
                if (map.ReplicaClosed)
                    ReplicateClosingMaps.Enqueue(map);
                else
                    map.Process();
            }
            while (!ReplicateClosingMaps.IsEmpty)
            {
                if (ReplicateClosingMaps.TryDequeue(out var map))
                    ReplicateMaps.Remove(map);
            }
            ProcessSandCity();
        }
        catch (Exception ex)
        {
            File.WriteAllText($".\\Log\\Error\\{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.txt", "TargetSite:\r\n" + ex.TargetSite?.ToString() + "\r\nHelpLink:\r\n" + ex.HelpLink + "\r\nInnerException:\r\n" + ex.InnerException?.ToString() + "\r\nSource:\r\n" + ex.Source + "\r\nMessage:\r\n" + ex.Message + "\r\nStackTrace:\r\n" + ex.StackTrace);
        }
    }

    public static void Initialize()
    {
        SecondaryObjects = new List<MapObject>();
        BackupObjects = new List<MapObject>();
        ReplicateMaps = new HashSet<Map>();
        ReplicateClosingMaps = new ConcurrentQueue<Map>();
        ActivationQueue = new ConcurrentQueue<MapObject>();
        DeactivationQueue = new ConcurrentQueue<MapObject>();
        ActiveObjects = new Dictionary<int, MapObject>();
        Objects = new Dictionary<int, MapObject>();
        Maps = new Dictionary<int, Map>();
        Players = new Dictionary<int, PlayerObject>();
        Monsters = new Dictionary<int, MonsterObject>();
        Pets = new Dictionary<int, PetObject>();
        Guards = new Dictionary<int, GuardObject>();
        Items = new Dictionary<int, ItemObject>();
        Traps = new Dictionary<int, TrapObject>();

        foreach (GameMap map in GameMap.DataSheet.Values)
        {
            Maps.Add(map.MapID * 16 + 1, new Map(map, 16_777_217));
        }

        foreach (Terrain terrain in Terrain.DataSheet.Values)
        {
            foreach (Map map in Maps.Values)
            {
                if (map.MapID != terrain.MapID)
                    continue;

                map.Terrain = terrain;
                map.Cells = new HashSet<MapObject>[map.MapSize.X, map.MapSize.Y];
                for (int i = 0; i < map.MapSize.X; i++)
                {
                    for (int j = 0; j < map.MapSize.Y; j++)
                    {
                        map.Cells[i, j] = new HashSet<MapObject>();
                    }
                }
            }
        }

        foreach (MapArea area in MapArea.DataSheet)
        {
            foreach (Map map in Maps.Values)
            {
                if (map.MapID == area.MapID)
                {
                    if (area.RegionType == AreaType.Resurrection)
                    {
                        map.ResurrectionArea = area;
                    }
                    if (area.RegionType == AreaType.RedName)
                    {
                        map.RedNameArea = area;
                    }
                    if (area.RegionType == AreaType.Teleportation)
                    {
                        map.TeleportationArea = area;
                    }
                    if (area.RegionType == AreaType.攻沙快捷)
                    {
                        map.攻沙快捷 = area;
                    }
                    if (area.RegionType == AreaType.传送区域沙左)
                    {
                        map.传送区域沙左 = area;
                    }
                    if (area.RegionType == AreaType.传送区域沙右)
                    {
                        map.传送区域沙右 = area;
                    }
                    if (area.RegionType == AreaType.传送区域皇宫)
                    {
                        map.传送区域皇宫 = area;
                    }
                    if (area.RegionType == AreaType.DemonTower1)
                    {
                        map.DemonTower1Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower2)
                    {
                        map.DemonTower2Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower3)
                    {
                        map.DemonTower3Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower4)
                    {
                        map.DemonTower4Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower5)
                    {
                        map.DemonTower5Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower6)
                    {
                        map.DemonTower6Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower7)
                    {
                        map.DemonTower7Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower8)
                    {
                        map.DemonTower8Area = area;
                    }
                    if (area.RegionType == AreaType.DemonTower9)
                    {
                        map.DemonTower9Area = area;
                    }
                    map.Areas.Add(area);
                    break;
                }
            }
        }
        foreach (TeleportGate gate in TeleportGate.DataSheet)
        {
            foreach (Map map in Maps.Values)
            {
                if (map.MapID == gate.MapID)
                    map.TeleportGates.Add(gate.GateID, gate);
            }
        }
        foreach (MapGuard guard in MapGuard.DataSheet)
        {
            foreach (Map map in Maps.Values)
            {
                if (map.MapID == guard.MapID)
                    map.Guards.Add(guard);
            }
        }
        foreach (MonsterSpawn spawn in MonsterSpawn.DataSheet)
        {
            foreach (Map map in Maps.Values)
            {
                if (map.MapID == spawn.MapID)
                    map.Spawns.Add(spawn);
            }
        }
        foreach (Map map in Maps.Values)
        {
            if (!map.QuestMap)
            {
                foreach (MonsterSpawn spawn in map.Spawns)
                {
                    if (spawn.Spawns == null) continue;
                    
                    Point[] locations = spawn.RangeCoordinates.ToArray();
                    foreach (MonsterSpawnInfo spawni in spawn.Spawns)
                    {
                        if (MonsterInfo.DataSheet.TryGetValue(spawni.MonsterName, out var value))
                        {
                            SMain.添加怪物数据(value);
                            int duration = spawni.RevivalInterval * 60 * 1000;
                            for (int i = 0; i < spawni.SpawnCount; i++)
                            {
                                new MonsterObject(value, map, duration, locations, forbidResurrection: false, 立即刷新: true);
                            }
                        }
                    }
                }
                foreach (MapGuard guard in map.Guards)
                {
                    if (GuardInfo.DataSheet.TryGetValue(guard.GuardID, out var gi))
                    {
                        new GuardObject(gi, map, guard.Direction, guard.Coordinates);
                    }
                }
            }
            else
            {
                map.TotalFixedMonsters = (uint)map.Spawns.Sum((MonsterSpawn O) => O.Spawns.Sum((MonsterSpawnInfo X) => X.SpawnCount));
            }
            SMain.添加地图数据(map);
        }
    }

    public static void RemoveItems()
    {
        foreach (ItemObject item in Items.Values)
        {
            item.Item?.Remove();
        }
        foreach (KeyValuePair<int, GameStore> item in GameStore.DataSheet)
        {
            foreach (ItemInfo item2 in item.Value.AvailableItems)
            {
                item2.Remove();
            }
        }
    }

    public static void AddObject(MapObject obj)
    {
        Objects.Add(obj.ObjectID, obj);
        switch (obj.ObjectType)
        {
            case GameObjectType.NPC:
                Guards.Add(obj.ObjectID, (GuardObject)obj);
                break;
            case GameObjectType.Player:
                Players.Add(obj.ObjectID, (PlayerObject)obj);
                break;
            case GameObjectType.Pet:
                Pets.Add(obj.ObjectID, (PetObject)obj);
                break;
            case GameObjectType.Monster:
                Monsters.Add(obj.ObjectID, (MonsterObject)obj);
                break;
            case GameObjectType.Trap:
                Traps.Add(obj.ObjectID, (TrapObject)obj);
                break;
            case GameObjectType.Item:
                Items.Add(obj.ObjectID, (ItemObject)obj);
                break;
        }
    }

    public static void RemoveObject(MapObject obj)
    {
        Objects.Remove(obj.ObjectID);
        switch (obj.ObjectType)
        {
            case GameObjectType.NPC:
                Guards.Remove(obj.ObjectID);
                break;
            case GameObjectType.Player:
                Players.Remove(obj.ObjectID);
                break;
            case GameObjectType.Pet:
                Pets.Remove(obj.ObjectID);
                break;
            case GameObjectType.Monster:
                Monsters.Remove(obj.ObjectID);
                break;
            case GameObjectType.Trap:
                Traps.Remove(obj.ObjectID);
                break;
            case GameObjectType.Item:
                Items.Remove(obj.ObjectID);
                break;
        }
    }

    public static void AddActiveObject(MapObject obj)
    {
        ActivationQueue.Enqueue(obj);
    }

    public static void RemoveActiveObject(MapObject obj)
    {
        DeactivationQueue.Enqueue(obj);
    }

    public static void AddSecondaryObject(MapObject obj)
    {
        BackupObjects.Add(obj);
    }

    public static Map GetMap(int id)
    {
        if (Maps.TryGetValue(id * 16 + 1, out var value))
            return value;
        return null;
    }

    static MapManager()
    {
        ObjectID = 268435456;
        TrapObjectID = 1073741824;
        ItemObjectID = 1342177280;
        沙城城门坐标 = new Point(1020, 506);
        皇宫下门坐标 = new Point(1079, 557);
        皇宫下门出口 = new Point(1078, 556);
        皇宫下门入口 = new Point(1265, 773);
        皇宫左门坐标 = new Point(1082, 557);
        皇宫左门出口 = new Point(1083, 556);
        皇宫左门入口 = new Point(1266, 773);
        皇宫上门坐标 = new Point(1071, 565);
        皇宫上门出口 = new Point(1070, 564);
        皇宫上门入口 = new Point(1254, 784);
        皇宫出口点一 = new Point(1257, 777);
        皇宫出口点二 = new Point(1258, 776);
        皇宫正门入口 = new Point(1258, 777);
        皇宫正门出口 = new Point(1074, 560);
        皇宫入口点左 = new Point(1076, 560);
        皇宫入口点中 = new Point(1075, 561);
        皇宫入口点右 = new Point(1074, 562);
        八卦坛坐标上 = new Point(1059, 591);
        八卦坛坐标下 = new Point(1054, 586);
        八卦坛坐标左 = new Point(1059, 586);
        八卦坛坐标右 = new Point(1054, 591);
        八卦坛坐标中 = new Point(1056, 588);
        八卦坛激活计时 = DateTime.MaxValue;
        SiegeGuilds = new HashSet<GuildInfo>();
    }
}
