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
    public static DateTime 宕机计时器;
    private static DateTime 沙城处理计时;
    private static DateTime 沙城处理计时1;

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

    public static byte 沙城节点;

    public static DateTime 通知时间;
    public static DateTime 通知时间2;
    public static DateTime 通知时间3;
    public static DateTime 通知时间4;
    public static DateTime 通知时间5;
    public static DateTime 通知时间6;
    public static DateTime 通知时间7;

    public static HashSet<GuildInfo> 攻城行会;

    private static void ProcessSandCity()
    {
        DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
        if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && Config.沙巴克每周攻沙时间 == 0)
        {
            Map map = GetMap(152);
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
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("The Siege of Shabak will begin in ten minutes, so be prepared", true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag = true;
                GuildInfo 行会数据 = null;
                foreach (Point item9 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item10 in map[item9])
                    {
                        if (!item10.Dead && item10 is PlayerObject 玩家实例10)
                        {
                            if (玩家实例10.Guild == null || !攻城行会.Contains(玩家实例10.Guild))
                            {
                                flag = false;
                                break;
                            }
                            if (行会数据 == null)
                            {
                                行会数据 = 玩家实例10.Guild;
                            }
                            else if (行会数据 != 玩家实例10.Guild)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item11 in 行会数据.Members)
                    {
                        item11.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"The siege of Shabak is over, [{行会数据}]Become the new Shabak Guild", true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("Shabak reset, about to start the siege, please get ready", true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
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
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"Gain the qualification guild to capture Shabak【{SystemInfo.Info.OccupyGuild.V}】", true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item12 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item12.Value);
                    SystemInfo.Info.申请行会.Remove(item12.Key);
                    NetworkManager.SendAnnouncement($"Gain the qualification guild to capture Shabak【{item12.Value}】", true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"Only the Shabak Guild is registered!The Shabak Siege is closed and the Guild is occupied【{SystemInfo.Info.OccupyGuild.V}】", true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("There are no siege guilds today, and the Shabak siege is closed!", true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("The Siege of Shabak begins! Warriors fight for honor!", true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("The gates of Shabak have been breached", true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = map.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag2 = true;
                GuildInfo 行会数据2 = null;
                foreach (Point item13 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item14 in map[item13])
                    {
                        if (!item14.Dead && item14 is PlayerObject 玩家实例11)
                        {
                            if (玩家实例11.Guild == null || !攻城行会.Contains(玩家实例11.Guild))
                            {
                                flag2 = false;
                                break;
                            }
                            if (行会数据2 == null)
                            {
                                行会数据2 = 玩家实例11.Guild;
                            }
                            else if (行会数据2 != 玩家实例11.Guild)
                            {
                                flag2 = false;
                                break;
                            }
                        }
                    }
                    if (!flag2)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据2) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据2 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据2;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据2.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag3 = true;
                GuildInfo 行会数据3 = null;
                foreach (Point item15 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item16 in map[item15])
                    {
                        if (!item16.Dead && item16 is PlayerObject 玩家实例12)
                        {
                            if (玩家实例12.Guild == null || !攻城行会.Contains(玩家实例12.Guild))
                            {
                                flag3 = false;
                                break;
                            }
                            if (行会数据3 == null)
                            {
                                行会数据3 = 玩家实例12.Guild;
                            }
                            else if (行会数据3 != 玩家实例12.Guild)
                            {
                                flag3 = false;
                                break;
                            }
                        }
                    }
                    if (!flag3)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据3))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据3;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (var member in 行会数据3.Members)
                    {
                        member.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"The siege of Shabak is over, [{行会数据3}]Become the new Shabak Guild", true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据3.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek == DayOfWeek.Monday && Config.沙巴克每周攻沙时间 == 1)
        {
            Map 地图实例3 = GetMap(152);
            foreach (MapObject item18 in 地图实例3[皇宫下门坐标].ToList())
            {
                if (!item18.Dead && item18 is PlayerObject 玩家实例13 && SEngine.CurrentTime > 玩家实例13.BusyTime)
                {
                    玩家实例13.Teleport(地图实例3, AreaType.Unknown, 皇宫下门入口);
                }
            }
            foreach (MapObject item19 in 地图实例3[皇宫上门坐标].ToList())
            {
                if (!item19.Dead && item19 is PlayerObject 玩家实例14 && SEngine.CurrentTime > 玩家实例14.BusyTime)
                {
                    玩家实例14.Teleport(地图实例3, AreaType.Unknown, 皇宫上门入口);
                }
            }
            foreach (MapObject item20 in 地图实例3[皇宫左门坐标].ToList())
            {
                if (!item20.Dead && item20 is PlayerObject 玩家实例15 && SEngine.CurrentTime > 玩家实例15.BusyTime)
                {
                    玩家实例15.Teleport(地图实例3, AreaType.Unknown, 皇宫左门入口);
                }
            }
            foreach (MapObject item21 in 地图实例3[皇宫出口点一].ToList())
            {
                if (!item21.Dead && item21 is PlayerObject 玩家实例16 && SEngine.CurrentTime > 玩家实例16.BusyTime)
                {
                    玩家实例16.Teleport(地图实例3, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item22 in 地图实例3[皇宫出口点二].ToList())
            {
                if (!item22.Dead && item22 is PlayerObject 玩家实例17 && SEngine.CurrentTime > 玩家实例17.BusyTime)
                {
                    玩家实例17.Teleport(地图实例3, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item23 in 地图实例3[皇宫入口点左].ToList())
            {
                if (!item23.Dead && item23 is PlayerObject 玩家实例18 && SEngine.CurrentTime > 玩家实例18.BusyTime && 玩家实例18.Guild != null && 玩家实例18.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例18.Teleport(地图实例3, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item24 in 地图实例3[皇宫入口点中].ToList())
            {
                if (!item24.Dead && item24 is PlayerObject 玩家实例19 && SEngine.CurrentTime > 玩家实例19.BusyTime && 玩家实例19.Guild != null && 玩家实例19.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例19.Teleport(地图实例3, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item25 in 地图实例3[皇宫入口点右].ToList())
            {
                if (!item25.Dead && item25 is PlayerObject 玩家实例20 && SEngine.CurrentTime > 玩家实例20.BusyTime && 玩家实例20.Guild != null && 玩家实例20.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例20.Teleport(地图实例3, AreaType.Unknown, 皇宫正门入口);
                }
            }
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("The Siege of Shabak will begin in ten minutes, so be prepared", true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag4 = true;
                GuildInfo 行会数据4 = null;
                foreach (Point item26 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item27 in 地图实例3[item26])
                    {
                        if (!item27.Dead && item27 is PlayerObject 玩家实例21)
                        {
                            if (玩家实例21.Guild == null || !攻城行会.Contains(玩家实例21.Guild))
                            {
                                flag4 = false;
                                break;
                            }
                            if (行会数据4 == null)
                            {
                                行会数据4 = 玩家实例21.Guild;
                            }
                            else if (行会数据4 != 玩家实例21.Guild)
                            {
                                flag4 = false;
                                break;
                            }
                        }
                    }
                    if (!flag4)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据4))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据4;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item28 in 行会数据4.Members)
                    {
                        item28.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"The siege of Shabak is over, [{行会数据4}]Become the new Shabak Guild", true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据4.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("Shabak reset, about to start the siege, please get ready", true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value4);
                MonsterObject 怪物实例2 = new MonsterObject(value4, 地图实例3, int.MaxValue, new Point[1] { 沙城城门坐标 }, true, true);
                怪物实例2.CurrentDirection = GameDirection.UpRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                沙城城门 = 怪物实例2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value5);
                怪物实例2 = new MonsterObject(value5, 地图实例3, int.MaxValue, new Point[1] { 皇宫上门坐标 }, true, true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                上方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value5, 地图实例3, int.MaxValue, new Point[1] { 皇宫下门坐标 }, true, true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                下方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value5, 地图实例3, int.MaxValue, new Point[1] { 皇宫左门坐标 }, true, true);
                怪物实例2.CurrentDirection = GameDirection.DownLeft;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                左方宫门 = 怪物实例2;
                GameBuff.DataSheet.TryGetValue(22300, out var value6);
                沙城城门.AddBuff(value6.ID, 沙城城门);
                上方宫门.AddBuff(value6.ID, 上方宫门);
                下方宫门.AddBuff(value6.ID, 下方宫门);
                左方宫门.AddBuff(value6.ID, 左方宫门);
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{SystemInfo.Info.OccupyGuild.V}】", true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item29 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item29.Value);
                    SystemInfo.Info.申请行会.Remove(item29.Key);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{item29.Value}】", true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"只有沙巴克行会报名!沙巴克攻城战关闭,占领行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("今日无攻城行会,沙巴克攻城战关闭!", rolling: true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("沙巴克攻城战开始!勇士们为了荣誉战斗吧!", rolling: true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("沙巴克城门已经被攻破", rolling: true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = 地图实例3.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag5 = true;
                GuildInfo 行会数据5 = null;
                foreach (Point item30 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item31 in 地图实例3[item30])
                    {
                        if (!item31.Dead && item31 is PlayerObject 玩家实例22)
                        {
                            if (玩家实例22.Guild == null || !攻城行会.Contains(玩家实例22.Guild))
                            {
                                flag5 = false;
                                break;
                            }
                            if (行会数据5 == null)
                            {
                                行会数据5 = 玩家实例22.Guild;
                            }
                            else if (行会数据5 != 玩家实例22.Guild)
                            {
                                flag5 = false;
                                break;
                            }
                        }
                    }
                    if (!flag5)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据5) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据5 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据5;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据5.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag6 = true;
                GuildInfo 行会数据6 = null;
                foreach (Point item32 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item33 in 地图实例3[item32])
                    {
                        if (!item33.Dead && item33 is PlayerObject 玩家实例23)
                        {
                            if (玩家实例23.Guild == null || !攻城行会.Contains(玩家实例23.Guild))
                            {
                                flag6 = false;
                                break;
                            }
                            if (行会数据6 == null)
                            {
                                行会数据6 = 玩家实例23.Guild;
                            }
                            else if (行会数据6 != 玩家实例23.Guild)
                            {
                                flag6 = false;
                                break;
                            }
                        }
                    }
                    if (!flag6)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据6))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据6;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item34 in 行会数据6.Members)
                    {
                        item34.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据6}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据6.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek.ToString() == "Tuesday" && Config.沙巴克每周攻沙时间 == 2)
        {
            Map 地图实例4 = GetMap(152);
            foreach (MapObject item35 in 地图实例4[皇宫下门坐标].ToList())
            {
                if (!item35.Dead && item35 is PlayerObject 玩家实例24 && SEngine.CurrentTime > 玩家实例24.BusyTime)
                {
                    玩家实例24.Teleport(地图实例4, AreaType.Unknown, 皇宫下门入口);
                }
            }
            foreach (MapObject item36 in 地图实例4[皇宫上门坐标].ToList())
            {
                if (!item36.Dead && item36 is PlayerObject 玩家实例25 && SEngine.CurrentTime > 玩家实例25.BusyTime)
                {
                    玩家实例25.Teleport(地图实例4, AreaType.Unknown, 皇宫上门入口);
                }
            }
            foreach (MapObject item37 in 地图实例4[皇宫左门坐标].ToList())
            {
                if (!item37.Dead && item37 is PlayerObject 玩家实例26 && SEngine.CurrentTime > 玩家实例26.BusyTime)
                {
                    玩家实例26.Teleport(地图实例4, AreaType.Unknown, 皇宫左门入口);
                }
            }
            foreach (MapObject item38 in 地图实例4[皇宫出口点一].ToList())
            {
                if (!item38.Dead && item38 is PlayerObject 玩家实例27 && SEngine.CurrentTime > 玩家实例27.BusyTime)
                {
                    玩家实例27.Teleport(地图实例4, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item39 in 地图实例4[皇宫出口点二].ToList())
            {
                if (!item39.Dead && item39 is PlayerObject 玩家实例28 && SEngine.CurrentTime > 玩家实例28.BusyTime)
                {
                    玩家实例28.Teleport(地图实例4, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item40 in 地图实例4[皇宫入口点左].ToList())
            {
                if (!item40.Dead && item40 is PlayerObject 玩家实例29 && SEngine.CurrentTime > 玩家实例29.BusyTime && 玩家实例29.Guild != null && 玩家实例29.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例29.Teleport(地图实例4, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item41 in 地图实例4[皇宫入口点中].ToList())
            {
                if (!item41.Dead && item41 is PlayerObject 玩家实例30 && SEngine.CurrentTime > 玩家实例30.BusyTime && 玩家实例30.Guild != null && 玩家实例30.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例30.Teleport(地图实例4, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item42 in 地图实例4[皇宫入口点右].ToList())
            {
                if (!item42.Dead && item42 is PlayerObject 玩家实例31 && SEngine.CurrentTime > 玩家实例31.BusyTime && 玩家实例31.Guild != null && 玩家实例31.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例31.Teleport(地图实例4, AreaType.Unknown, 皇宫正门入口);
                }
            }
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("沙巴克攻城战将在十分钟后开始, 请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag7 = true;
                GuildInfo 行会数据7 = null;
                foreach (Point item43 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item44 in 地图实例4[item43])
                    {
                        if (!item44.Dead && item44 is PlayerObject 玩家实例32)
                        {
                            if (玩家实例32.Guild == null || !攻城行会.Contains(玩家实例32.Guild))
                            {
                                flag7 = false;
                                break;
                            }
                            if (行会数据7 == null)
                            {
                                行会数据7 = 玩家实例32.Guild;
                            }
                            else if (行会数据7 != 玩家实例32.Guild)
                            {
                                flag7 = false;
                                break;
                            }
                        }
                    }
                    if (!flag7)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据7))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据7;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item45 in 行会数据7.Members)
                    {
                        item45.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据7}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据7.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("沙巴克重置,即将开始进入攻城战,请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value7);
                MonsterObject 怪物实例2 = new MonsterObject(value7, 地图实例4, int.MaxValue, new Point[1] { 沙城城门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.UpRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                沙城城门 = 怪物实例2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value8);
                怪物实例2 = new MonsterObject(value8, 地图实例4, int.MaxValue, new Point[1] { 皇宫上门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                上方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value8, 地图实例4, int.MaxValue, new Point[1] { 皇宫下门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                下方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value8, 地图实例4, int.MaxValue, new Point[1] { 皇宫左门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownLeft;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                左方宫门 = 怪物实例2;
                GameBuff.DataSheet.TryGetValue(22300, out var value9);
                沙城城门.AddBuff(value9.ID, 沙城城门);
                上方宫门.AddBuff(value9.ID, 上方宫门);
                下方宫门.AddBuff(value9.ID, 下方宫门);
                左方宫门.AddBuff(value9.ID, 左方宫门);
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item46 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item46.Value);
                    SystemInfo.Info.申请行会.Remove(item46.Key);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{item46.Value}】", rolling: true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"只有沙巴克行会报名!沙巴克攻城战关闭,占领行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("今日无攻城行会,沙巴克攻城战关闭!", rolling: true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("沙巴克攻城战开始!勇士们为了荣誉战斗吧!", rolling: true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("沙巴克城门已经被攻破", rolling: true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = 地图实例4.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag8 = true;
                GuildInfo 行会数据8 = null;
                foreach (Point item47 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item48 in 地图实例4[item47])
                    {
                        if (!item48.Dead && item48 is PlayerObject 玩家实例33)
                        {
                            if (玩家实例33.Guild == null || !攻城行会.Contains(玩家实例33.Guild))
                            {
                                flag8 = false;
                                break;
                            }
                            if (行会数据8 == null)
                            {
                                行会数据8 = 玩家实例33.Guild;
                            }
                            else if (行会数据8 != 玩家实例33.Guild)
                            {
                                flag8 = false;
                                break;
                            }
                        }
                    }
                    if (!flag8)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据8) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据8 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据8;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据8.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag9 = true;
                GuildInfo 行会数据9 = null;
                foreach (Point item49 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item50 in 地图实例4[item49])
                    {
                        if (!item50.Dead && item50 is PlayerObject 玩家实例34)
                        {
                            if (玩家实例34.Guild == null || !攻城行会.Contains(玩家实例34.Guild))
                            {
                                flag9 = false;
                                break;
                            }
                            if (行会数据9 == null)
                            {
                                行会数据9 = 玩家实例34.Guild;
                            }
                            else if (行会数据9 != 玩家实例34.Guild)
                            {
                                flag9 = false;
                                break;
                            }
                        }
                    }
                    if (!flag9)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据9))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据9;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item51 in 行会数据9.Members)
                    {
                        item51.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据9}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据9.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek.ToString() == "Wednesday" && Config.沙巴克每周攻沙时间 == 3)
        {
            Map 地图实例5 = GetMap(152);
            foreach (MapObject item52 in 地图实例5[皇宫下门坐标].ToList())
            {
                if (!item52.Dead && item52 is PlayerObject 玩家实例35 && SEngine.CurrentTime > 玩家实例35.BusyTime)
                {
                    玩家实例35.Teleport(地图实例5, AreaType.Unknown, 皇宫下门入口);
                }
            }
            foreach (MapObject item53 in 地图实例5[皇宫上门坐标].ToList())
            {
                if (!item53.Dead && item53 is PlayerObject 玩家实例36 && SEngine.CurrentTime > 玩家实例36.BusyTime)
                {
                    玩家实例36.Teleport(地图实例5, AreaType.Unknown, 皇宫上门入口);
                }
            }
            foreach (MapObject item54 in 地图实例5[皇宫左门坐标].ToList())
            {
                if (!item54.Dead && item54 is PlayerObject 玩家实例37 && SEngine.CurrentTime > 玩家实例37.BusyTime)
                {
                    玩家实例37.Teleport(地图实例5, AreaType.Unknown, 皇宫左门入口);
                }
            }
            foreach (MapObject item55 in 地图实例5[皇宫出口点一].ToList())
            {
                if (!item55.Dead && item55 is PlayerObject 玩家实例38 && SEngine.CurrentTime > 玩家实例38.BusyTime)
                {
                    玩家实例38.Teleport(地图实例5, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item56 in 地图实例5[皇宫出口点二].ToList())
            {
                if (!item56.Dead && item56 is PlayerObject 玩家实例39 && SEngine.CurrentTime > 玩家实例39.BusyTime)
                {
                    玩家实例39.Teleport(地图实例5, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item57 in 地图实例5[皇宫入口点左].ToList())
            {
                if (!item57.Dead && item57 is PlayerObject 玩家实例40 && SEngine.CurrentTime > 玩家实例40.BusyTime && 玩家实例40.Guild != null && 玩家实例40.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例40.Teleport(地图实例5, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item58 in 地图实例5[皇宫入口点中].ToList())
            {
                if (!item58.Dead && item58 is PlayerObject 玩家实例41 && SEngine.CurrentTime > 玩家实例41.BusyTime && 玩家实例41.Guild != null && 玩家实例41.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例41.Teleport(地图实例5, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item59 in 地图实例5[皇宫入口点右].ToList())
            {
                if (!item59.Dead && item59 is PlayerObject 玩家实例42 && SEngine.CurrentTime > 玩家实例42.BusyTime && 玩家实例42.Guild != null && 玩家实例42.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例42.Teleport(地图实例5, AreaType.Unknown, 皇宫正门入口);
                }
            }
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("沙巴克攻城战将在十分钟后开始, 请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag10 = true;
                GuildInfo 行会数据10 = null;
                foreach (Point item60 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item61 in 地图实例5[item60])
                    {
                        if (!item61.Dead && item61 is PlayerObject 玩家实例43)
                        {
                            if (玩家实例43.Guild == null || !攻城行会.Contains(玩家实例43.Guild))
                            {
                                flag10 = false;
                                break;
                            }
                            if (行会数据10 == null)
                            {
                                行会数据10 = 玩家实例43.Guild;
                            }
                            else if (行会数据10 != 玩家实例43.Guild)
                            {
                                flag10 = false;
                                break;
                            }
                        }
                    }
                    if (!flag10)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据10))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据10;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item62 in 行会数据10.Members)
                    {
                        item62.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据10}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据10.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("沙巴克重置,即将开始进入攻城战,请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value10);
                MonsterObject 怪物实例2 = new MonsterObject(value10, 地图实例5, int.MaxValue, new Point[1] { 沙城城门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.UpRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                沙城城门 = 怪物实例2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value11);
                怪物实例2 = new MonsterObject(value11, 地图实例5, int.MaxValue, new Point[1] { 皇宫上门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                上方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value11, 地图实例5, int.MaxValue, new Point[1] { 皇宫下门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                下方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value11, 地图实例5, int.MaxValue, new Point[1] { 皇宫左门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownLeft;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                左方宫门 = 怪物实例2;
                GameBuff.DataSheet.TryGetValue(22300, out var value12);
                沙城城门.AddBuff(value12.ID, 沙城城门);
                上方宫门.AddBuff(value12.ID, 上方宫门);
                下方宫门.AddBuff(value12.ID, 下方宫门);
                左方宫门.AddBuff(value12.ID, 左方宫门);
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item63 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item63.Value);
                    SystemInfo.Info.申请行会.Remove(item63.Key);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{item63.Value}】", rolling: true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"只有沙巴克行会报名!沙巴克攻城战关闭,占领行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("今日无攻城行会,沙巴克攻城战关闭!", rolling: true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("沙巴克攻城战开始!勇士们为了荣誉战斗吧!", rolling: true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("沙巴克城门已经被攻破", rolling: true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = 地图实例5.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag11 = true;
                GuildInfo 行会数据11 = null;
                foreach (Point item64 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item65 in 地图实例5[item64])
                    {
                        if (!item65.Dead && item65 is PlayerObject 玩家实例44)
                        {
                            if (玩家实例44.Guild == null || !攻城行会.Contains(玩家实例44.Guild))
                            {
                                flag11 = false;
                                break;
                            }
                            if (行会数据11 == null)
                            {
                                行会数据11 = 玩家实例44.Guild;
                            }
                            else if (行会数据11 != 玩家实例44.Guild)
                            {
                                flag11 = false;
                                break;
                            }
                        }
                    }
                    if (!flag11)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据11) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据11 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据11;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据11.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag12 = true;
                GuildInfo 行会数据12 = null;
                foreach (Point item66 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item67 in 地图实例5[item66])
                    {
                        if (!item67.Dead && item67 is PlayerObject 玩家实例45)
                        {
                            if (玩家实例45.Guild == null || !攻城行会.Contains(玩家实例45.Guild))
                            {
                                flag12 = false;
                                break;
                            }
                            if (行会数据12 == null)
                            {
                                行会数据12 = 玩家实例45.Guild;
                            }
                            else if (行会数据12 != 玩家实例45.Guild)
                            {
                                flag12 = false;
                                break;
                            }
                        }
                    }
                    if (!flag12)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据12))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据12;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item68 in 行会数据12.Members)
                    {
                        item68.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据12}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据12.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek.ToString() == "Thursday" && Config.沙巴克每周攻沙时间 == 4)
        {
            Map 地图实例6 = GetMap(152);
            foreach (MapObject item69 in 地图实例6[皇宫下门坐标].ToList())
            {
                if (!item69.Dead && item69 is PlayerObject 玩家实例46 && SEngine.CurrentTime > 玩家实例46.BusyTime)
                {
                    玩家实例46.Teleport(地图实例6, AreaType.Unknown, 皇宫下门入口);
                }
            }
            foreach (MapObject item70 in 地图实例6[皇宫上门坐标].ToList())
            {
                if (!item70.Dead && item70 is PlayerObject 玩家实例47 && SEngine.CurrentTime > 玩家实例47.BusyTime)
                {
                    玩家实例47.Teleport(地图实例6, AreaType.Unknown, 皇宫上门入口);
                }
            }
            foreach (MapObject item71 in 地图实例6[皇宫左门坐标].ToList())
            {
                if (!item71.Dead && item71 is PlayerObject 玩家实例48 && SEngine.CurrentTime > 玩家实例48.BusyTime)
                {
                    玩家实例48.Teleport(地图实例6, AreaType.Unknown, 皇宫左门入口);
                }
            }
            foreach (MapObject item72 in 地图实例6[皇宫出口点一].ToList())
            {
                if (!item72.Dead && item72 is PlayerObject 玩家实例49 && SEngine.CurrentTime > 玩家实例49.BusyTime)
                {
                    玩家实例49.Teleport(地图实例6, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item73 in 地图实例6[皇宫出口点二].ToList())
            {
                if (!item73.Dead && item73 is PlayerObject 玩家实例50 && SEngine.CurrentTime > 玩家实例50.BusyTime)
                {
                    玩家实例50.Teleport(地图实例6, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item74 in 地图实例6[皇宫入口点左].ToList())
            {
                if (!item74.Dead && item74 is PlayerObject 玩家实例51 && SEngine.CurrentTime > 玩家实例51.BusyTime && 玩家实例51.Guild != null && 玩家实例51.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例51.Teleport(地图实例6, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item75 in 地图实例6[皇宫入口点中].ToList())
            {
                if (!item75.Dead && item75 is PlayerObject 玩家实例52 && SEngine.CurrentTime > 玩家实例52.BusyTime && 玩家实例52.Guild != null && 玩家实例52.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例52.Teleport(地图实例6, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item76 in 地图实例6[皇宫入口点右].ToList())
            {
                if (!item76.Dead && item76 is PlayerObject 玩家实例53 && SEngine.CurrentTime > 玩家实例53.BusyTime && 玩家实例53.Guild != null && 玩家实例53.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例53.Teleport(地图实例6, AreaType.Unknown, 皇宫正门入口);
                }
            }
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("沙巴克攻城战将在十分钟后开始, 请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag13 = true;
                GuildInfo 行会数据13 = null;
                foreach (Point item77 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item78 in 地图实例6[item77])
                    {
                        if (!item78.Dead && item78 is PlayerObject 玩家实例54)
                        {
                            if (玩家实例54.Guild == null || !攻城行会.Contains(玩家实例54.Guild))
                            {
                                flag13 = false;
                                break;
                            }
                            if (行会数据13 == null)
                            {
                                行会数据13 = 玩家实例54.Guild;
                            }
                            else if (行会数据13 != 玩家实例54.Guild)
                            {
                                flag13 = false;
                                break;
                            }
                        }
                    }
                    if (!flag13)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据13))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据13;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item79 in 行会数据13.Members)
                    {
                        item79.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据13}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据13.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("沙巴克重置,即将开始进入攻城战,请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value13);
                MonsterObject 怪物实例2 = new MonsterObject(value13, 地图实例6, int.MaxValue, new Point[1] { 沙城城门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.UpRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                沙城城门 = 怪物实例2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value14);
                怪物实例2 = new MonsterObject(value14, 地图实例6, int.MaxValue, new Point[1] { 皇宫上门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                上方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value14, 地图实例6, int.MaxValue, new Point[1] { 皇宫下门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                下方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value14, 地图实例6, int.MaxValue, new Point[1] { 皇宫左门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownLeft;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                左方宫门 = 怪物实例2;
                GameBuff.DataSheet.TryGetValue(22300, out var value15);
                沙城城门.AddBuff(value15.ID, 沙城城门);
                上方宫门.AddBuff(value15.ID, 上方宫门);
                下方宫门.AddBuff(value15.ID, 下方宫门);
                左方宫门.AddBuff(value15.ID, 左方宫门);
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item80 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item80.Value);
                    SystemInfo.Info.申请行会.Remove(item80.Key);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{item80.Value}】", rolling: true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"只有沙巴克行会报名!沙巴克攻城战关闭,占领行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("今日无攻城行会,沙巴克攻城战关闭!", rolling: true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("沙巴克攻城战开始!勇士们为了荣誉战斗吧!", rolling: true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("沙巴克城门已经被攻破", rolling: true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = 地图实例6.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag14 = true;
                GuildInfo 行会数据14 = null;
                foreach (Point item81 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item82 in 地图实例6[item81])
                    {
                        if (!item82.Dead && item82 is PlayerObject 玩家实例55)
                        {
                            if (玩家实例55.Guild == null || !攻城行会.Contains(玩家实例55.Guild))
                            {
                                flag14 = false;
                                break;
                            }
                            if (行会数据14 == null)
                            {
                                行会数据14 = 玩家实例55.Guild;
                            }
                            else if (行会数据14 != 玩家实例55.Guild)
                            {
                                flag14 = false;
                                break;
                            }
                        }
                    }
                    if (!flag14)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据14) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据14 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据14;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据14.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag15 = true;
                GuildInfo 行会数据15 = null;
                foreach (Point item83 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item84 in 地图实例6[item83])
                    {
                        if (!item84.Dead && item84 is PlayerObject 玩家实例56)
                        {
                            if (玩家实例56.Guild == null || !攻城行会.Contains(玩家实例56.Guild))
                            {
                                flag15 = false;
                                break;
                            }
                            if (行会数据15 == null)
                            {
                                行会数据15 = 玩家实例56.Guild;
                            }
                            else if (行会数据15 != 玩家实例56.Guild)
                            {
                                flag15 = false;
                                break;
                            }
                        }
                    }
                    if (!flag15)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据15))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据15;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item85 in 行会数据15.Members)
                    {
                        item85.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据15}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据15.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek.ToString() == "Friday" && Config.沙巴克每周攻沙时间 == 5)
        {
            Map 地图实例7 = GetMap(152);
            foreach (MapObject item86 in 地图实例7[皇宫下门坐标].ToList())
            {
                if (!item86.Dead && item86 is PlayerObject 玩家实例57 && SEngine.CurrentTime > 玩家实例57.BusyTime)
                {
                    玩家实例57.Teleport(地图实例7, AreaType.Unknown, 皇宫下门入口);
                }
            }
            foreach (MapObject item87 in 地图实例7[皇宫上门坐标].ToList())
            {
                if (!item87.Dead && item87 is PlayerObject 玩家实例58 && SEngine.CurrentTime > 玩家实例58.BusyTime)
                {
                    玩家实例58.Teleport(地图实例7, AreaType.Unknown, 皇宫上门入口);
                }
            }
            foreach (MapObject item88 in 地图实例7[皇宫左门坐标].ToList())
            {
                if (!item88.Dead && item88 is PlayerObject 玩家实例59 && SEngine.CurrentTime > 玩家实例59.BusyTime)
                {
                    玩家实例59.Teleport(地图实例7, AreaType.Unknown, 皇宫左门入口);
                }
            }
            foreach (MapObject item89 in 地图实例7[皇宫出口点一].ToList())
            {
                if (!item89.Dead && item89 is PlayerObject 玩家实例60 && SEngine.CurrentTime > 玩家实例60.BusyTime)
                {
                    玩家实例60.Teleport(地图实例7, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item90 in 地图实例7[皇宫出口点二].ToList())
            {
                if (!item90.Dead && item90 is PlayerObject 玩家实例61 && SEngine.CurrentTime > 玩家实例61.BusyTime)
                {
                    玩家实例61.Teleport(地图实例7, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item91 in 地图实例7[皇宫入口点左].ToList())
            {
                if (!item91.Dead && item91 is PlayerObject 玩家实例62 && SEngine.CurrentTime > 玩家实例62.BusyTime && 玩家实例62.Guild != null && 玩家实例62.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例62.Teleport(地图实例7, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item92 in 地图实例7[皇宫入口点中].ToList())
            {
                if (!item92.Dead && item92 is PlayerObject 玩家实例63 && SEngine.CurrentTime > 玩家实例63.BusyTime && 玩家实例63.Guild != null && 玩家实例63.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例63.Teleport(地图实例7, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item93 in 地图实例7[皇宫入口点右].ToList())
            {
                if (!item93.Dead && item93 is PlayerObject 玩家实例64 && SEngine.CurrentTime > 玩家实例64.BusyTime && 玩家实例64.Guild != null && 玩家实例64.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例64.Teleport(地图实例7, AreaType.Unknown, 皇宫正门入口);
                }
            }
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("沙巴克攻城战将在十分钟后开始, 请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag16 = true;
                GuildInfo 行会数据16 = null;
                foreach (Point item94 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item95 in 地图实例7[item94])
                    {
                        if (!item95.Dead && item95 is PlayerObject 玩家实例65)
                        {
                            if (玩家实例65.Guild == null || !攻城行会.Contains(玩家实例65.Guild))
                            {
                                flag16 = false;
                                break;
                            }
                            if (行会数据16 == null)
                            {
                                行会数据16 = 玩家实例65.Guild;
                            }
                            else if (行会数据16 != 玩家实例65.Guild)
                            {
                                flag16 = false;
                                break;
                            }
                        }
                    }
                    if (!flag16)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据16))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据16;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item96 in 行会数据16.Members)
                    {
                        item96.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据16}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据16.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("沙巴克重置,即将开始进入攻城战,请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value16);
                MonsterObject 怪物实例2 = new MonsterObject(value16, 地图实例7, int.MaxValue, new Point[1] { 沙城城门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.UpRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                沙城城门 = 怪物实例2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value17);
                怪物实例2 = new MonsterObject(value17, 地图实例7, int.MaxValue, new Point[1] { 皇宫上门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                上方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value17, 地图实例7, int.MaxValue, new Point[1] { 皇宫下门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                下方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value17, 地图实例7, int.MaxValue, new Point[1] { 皇宫左门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownLeft;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                左方宫门 = 怪物实例2;
                GameBuff.DataSheet.TryGetValue(22300, out var value18);
                沙城城门.AddBuff(value18.ID, 沙城城门);
                上方宫门.AddBuff(value18.ID, 上方宫门);
                下方宫门.AddBuff(value18.ID, 下方宫门);
                左方宫门.AddBuff(value18.ID, 左方宫门);
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item97 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item97.Value);
                    SystemInfo.Info.申请行会.Remove(item97.Key);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{item97.Value}】", rolling: true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"只有沙巴克行会报名!沙巴克攻城战关闭,占领行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("今日无攻城行会,沙巴克攻城战关闭!", rolling: true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("沙巴克攻城战开始!勇士们为了荣誉战斗吧!", rolling: true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("沙巴克城门已经被攻破", rolling: true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = 地图实例7.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag17 = true;
                GuildInfo 行会数据17 = null;
                foreach (Point item98 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item99 in 地图实例7[item98])
                    {
                        if (!item99.Dead && item99 is PlayerObject 玩家实例66)
                        {
                            if (玩家实例66.Guild == null || !攻城行会.Contains(玩家实例66.Guild))
                            {
                                flag17 = false;
                                break;
                            }
                            if (行会数据17 == null)
                            {
                                行会数据17 = 玩家实例66.Guild;
                            }
                            else if (行会数据17 != 玩家实例66.Guild)
                            {
                                flag17 = false;
                                break;
                            }
                        }
                    }
                    if (!flag17)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据17) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据17 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据17;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据17.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag18 = true;
                GuildInfo 行会数据18 = null;
                foreach (Point item100 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item101 in 地图实例7[item100])
                    {
                        if (!item101.Dead && item101 is PlayerObject 玩家实例67)
                        {
                            if (玩家实例67.Guild == null || !攻城行会.Contains(玩家实例67.Guild))
                            {
                                flag18 = false;
                                break;
                            }
                            if (行会数据18 == null)
                            {
                                行会数据18 = 玩家实例67.Guild;
                            }
                            else if (行会数据18 != 玩家实例67.Guild)
                            {
                                flag18 = false;
                                break;
                            }
                        }
                    }
                    if (!flag18)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据18))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据18;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item102 in 行会数据18.Members)
                    {
                        item102.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据18}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据18.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Config.沙巴克开启 && dayOfWeek.ToString() == "Saturday" && Config.沙巴克每周攻沙时间 == 6)
        {
            Map 地图实例8 = GetMap(152);
            foreach (MapObject item103 in 地图实例8[皇宫下门坐标].ToList())
            {
                if (!item103.Dead && item103 is PlayerObject 玩家实例68 && SEngine.CurrentTime > 玩家实例68.BusyTime)
                {
                    玩家实例68.Teleport(地图实例8, AreaType.Unknown, 皇宫下门入口);
                }
            }
            foreach (MapObject item104 in 地图实例8[皇宫上门坐标].ToList())
            {
                if (!item104.Dead && item104 is PlayerObject 玩家实例69 && SEngine.CurrentTime > 玩家实例69.BusyTime)
                {
                    玩家实例69.Teleport(地图实例8, AreaType.Unknown, 皇宫上门入口);
                }
            }
            foreach (MapObject item105 in 地图实例8[皇宫左门坐标].ToList())
            {
                if (!item105.Dead && item105 is PlayerObject 玩家实例70 && SEngine.CurrentTime > 玩家实例70.BusyTime)
                {
                    玩家实例70.Teleport(地图实例8, AreaType.Unknown, 皇宫左门入口);
                }
            }
            foreach (MapObject item106 in 地图实例8[皇宫出口点一].ToList())
            {
                if (!item106.Dead && item106 is PlayerObject 玩家实例71 && SEngine.CurrentTime > 玩家实例71.BusyTime)
                {
                    玩家实例71.Teleport(地图实例8, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item107 in 地图实例8[皇宫出口点二].ToList())
            {
                if (!item107.Dead && item107 is PlayerObject 玩家实例72 && SEngine.CurrentTime > 玩家实例72.BusyTime)
                {
                    玩家实例72.Teleport(地图实例8, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item108 in 地图实例8[皇宫入口点左].ToList())
            {
                if (!item108.Dead && item108 is PlayerObject 玩家实例73 && SEngine.CurrentTime > 玩家实例73.BusyTime && 玩家实例73.Guild != null && 玩家实例73.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例73.Teleport(地图实例8, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item109 in 地图实例8[皇宫入口点中].ToList())
            {
                if (!item109.Dead && item109 is PlayerObject 玩家实例74 && SEngine.CurrentTime > 玩家实例74.BusyTime && 玩家实例74.Guild != null && 玩家实例74.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例74.Teleport(地图实例8, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item110 in 地图实例8[皇宫入口点右].ToList())
            {
                if (!item110.Dead && item110 is PlayerObject 玩家实例75 && SEngine.CurrentTime > 玩家实例75.BusyTime && 玩家实例75.Guild != null && 玩家实例75.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例75.Teleport(地图实例8, AreaType.Unknown, 皇宫正门入口);
                }
            }
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("沙巴克攻城战将在十分钟后开始, 请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag19 = true;
                GuildInfo 行会数据19 = null;
                foreach (Point item111 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item112 in 地图实例8[item111])
                    {
                        if (!item112.Dead && item112 is PlayerObject 玩家实例76)
                        {
                            if (玩家实例76.Guild == null || !攻城行会.Contains(玩家实例76.Guild))
                            {
                                flag19 = false;
                                break;
                            }
                            if (行会数据19 == null)
                            {
                                行会数据19 = 玩家实例76.Guild;
                            }
                            else if (行会数据19 != 玩家实例76.Guild)
                            {
                                flag19 = false;
                                break;
                            }
                        }
                    }
                    if (!flag19)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据19))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据19;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item113 in 行会数据19.Members)
                    {
                        item113.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据19}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据19.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("沙巴克重置,即将开始进入攻城战,请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value19);
                MonsterObject 怪物实例2 = new MonsterObject(value19, 地图实例8, int.MaxValue, new Point[1] { 沙城城门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.UpRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                沙城城门 = 怪物实例2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value20);
                怪物实例2 = new MonsterObject(value20, 地图实例8, int.MaxValue, new Point[1] { 皇宫上门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                上方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value20, 地图实例8, int.MaxValue, new Point[1] { 皇宫下门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                下方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value20, 地图实例8, int.MaxValue, new Point[1] { 皇宫左门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownLeft;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                左方宫门 = 怪物实例2;
                GameBuff.DataSheet.TryGetValue(22300, out var value21);
                沙城城门.AddBuff(value21.ID, 沙城城门);
                上方宫门.AddBuff(value21.ID, 上方宫门);
                下方宫门.AddBuff(value21.ID, 下方宫门);
                左方宫门.AddBuff(value21.ID, 左方宫门);
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item114 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item114.Value);
                    SystemInfo.Info.申请行会.Remove(item114.Key);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{item114.Value}】", rolling: true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"只有沙巴克行会报名!沙巴克攻城战关闭,占领行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("今日无攻城行会,沙巴克攻城战关闭!", rolling: true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("沙巴克攻城战开始!勇士们为了荣誉战斗吧!", rolling: true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("沙巴克城门已经被攻破", rolling: true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = 地图实例8.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag20 = true;
                GuildInfo 行会数据20 = null;
                foreach (Point item115 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item116 in 地图实例8[item115])
                    {
                        if (!item116.Dead && item116 is PlayerObject 玩家实例77)
                        {
                            if (玩家实例77.Guild == null || !攻城行会.Contains(玩家实例77.Guild))
                            {
                                flag20 = false;
                                break;
                            }
                            if (行会数据20 == null)
                            {
                                行会数据20 = 玩家实例77.Guild;
                            }
                            else if (行会数据20 != 玩家实例77.Guild)
                            {
                                flag20 = false;
                                break;
                            }
                        }
                    }
                    if (!flag20)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据20) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据20 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据20;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据20.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag21 = true;
                GuildInfo 行会数据21 = null;
                foreach (Point item117 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item118 in 地图实例8[item117])
                    {
                        if (!item118.Dead && item118 is PlayerObject 玩家实例78)
                        {
                            if (玩家实例78.Guild == null || !攻城行会.Contains(玩家实例78.Guild))
                            {
                                flag21 = false;
                                break;
                            }
                            if (行会数据21 == null)
                            {
                                行会数据21 = 玩家实例78.Guild;
                            }
                            else if (行会数据21 != 玩家实例78.Guild)
                            {
                                flag21 = false;
                                break;
                            }
                        }
                    }
                    if (!flag21)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据21))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据21;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item119 in 行会数据21.Members)
                    {
                        item119.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据21}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据21.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
        else
        {
            if (SEngine.CurrentTime.Hour + 2 < Config.沙巴克开启 || !(dayOfWeek.ToString() == "Sunday") || Config.沙巴克每周攻沙时间 != 7)
            {
                return;
            }
            Map 地图实例9 = GetMap(152);
            foreach (MapObject item120 in 地图实例9[皇宫下门坐标].ToList())
            {
                if (!item120.Dead && item120 is PlayerObject 玩家实例79 && SEngine.CurrentTime > 玩家实例79.BusyTime)
                {
                    玩家实例79.Teleport(地图实例9, AreaType.Unknown, 皇宫下门入口);
                }
            }
            foreach (MapObject item121 in 地图实例9[皇宫上门坐标].ToList())
            {
                if (!item121.Dead && item121 is PlayerObject 玩家实例80 && SEngine.CurrentTime > 玩家实例80.BusyTime)
                {
                    玩家实例80.Teleport(地图实例9, AreaType.Unknown, 皇宫上门入口);
                }
            }
            foreach (MapObject item122 in 地图实例9[皇宫左门坐标].ToList())
            {
                if (!item122.Dead && item122 is PlayerObject 玩家实例81 && SEngine.CurrentTime > 玩家实例81.BusyTime)
                {
                    玩家实例81.Teleport(地图实例9, AreaType.Unknown, 皇宫左门入口);
                }
            }
            foreach (MapObject item123 in 地图实例9[皇宫出口点一].ToList())
            {
                if (!item123.Dead && item123 is PlayerObject 玩家实例82 && SEngine.CurrentTime > 玩家实例82.BusyTime)
                {
                    玩家实例82.Teleport(地图实例9, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item124 in 地图实例9[皇宫出口点二].ToList())
            {
                if (!item124.Dead && item124 is PlayerObject 玩家实例83 && SEngine.CurrentTime > 玩家实例83.BusyTime)
                {
                    玩家实例83.Teleport(地图实例9, AreaType.Unknown, 皇宫正门出口);
                }
            }
            foreach (MapObject item125 in 地图实例9[皇宫入口点左].ToList())
            {
                if (!item125.Dead && item125 is PlayerObject 玩家实例84 && SEngine.CurrentTime > 玩家实例84.BusyTime && 玩家实例84.Guild != null && 玩家实例84.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例84.Teleport(地图实例9, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item126 in 地图实例9[皇宫入口点中].ToList())
            {
                if (!item126.Dead && item126 is PlayerObject 玩家实例85 && SEngine.CurrentTime > 玩家实例85.BusyTime && 玩家实例85.Guild != null && 玩家实例85.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例85.Teleport(地图实例9, AreaType.Unknown, 皇宫正门入口);
                }
            }
            foreach (MapObject item127 in 地图实例9[皇宫入口点右].ToList())
            {
                if (!item127.Dead && item127 is PlayerObject 玩家实例86 && SEngine.CurrentTime > 玩家实例86.BusyTime && 玩家实例86.Guild != null && 玩家实例86.Guild == SystemInfo.Info.OccupyGuild.V)
                {
                    玩家实例86.Teleport(地图实例9, AreaType.Unknown, 皇宫正门入口);
                }
            }
            if (!(SEngine.CurrentTime > 沙城处理计时))
            {
                return;
            }
            if (SEngine.CurrentTime.Hour + 1 < Config.沙巴克开启)
            {
                沙城节点 = 0;
            }
            if (SEngine.CurrentTime.Hour + 1 == Config.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && 沙城节点 == 0)
            {
                NetworkManager.SendAnnouncement("沙巴克攻城战将在十分钟后开始, 请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (Config.沙巴克停止开关 == 1)
            {
                沙城节点 = 0;
                Config.沙巴克停止开关 = 0;
                bool flag22 = true;
                GuildInfo 行会数据22 = null;
                foreach (Point item128 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item129 in 地图实例9[item128])
                    {
                        if (!item129.Dead && item129 is PlayerObject 玩家实例87)
                        {
                            if (玩家实例87.Guild == null || !攻城行会.Contains(玩家实例87.Guild))
                            {
                                flag22 = false;
                                break;
                            }
                            if (行会数据22 == null)
                            {
                                行会数据22 = 玩家实例87.Guild;
                            }
                            else if (行会数据22 != 玩家实例87.Guild)
                            {
                                flag22 = false;
                                break;
                            }
                        }
                    }
                    if (!flag22)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据22))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据22;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item130 in 行会数据22.Members)
                    {
                        item130.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据22}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据22.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && Config.沙巴克重置系统 == 1)
            {
                SystemInfo.Info.OccupyGuild.V = null;
                Config.沙巴克重置系统 = 0;
                NetworkManager.SendAnnouncement("沙巴克重置,即将开始进入攻城战,请做好准备", rolling: true);
                沙城节点 = 1;
            }
            if (沙城节点 == 1)
            {
                沙城节点 = 2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value22);
                MonsterObject 怪物实例2 = new MonsterObject(value22, 地图实例9, int.MaxValue, new Point[1] { 沙城城门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.UpRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                沙城城门 = 怪物实例2;
                MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value23);
                怪物实例2 = new MonsterObject(value23, 地图实例9, int.MaxValue, new Point[1] { 皇宫上门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                上方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value23, 地图实例9, int.MaxValue, new Point[1] { 皇宫下门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownRight;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                下方宫门 = 怪物实例2;
                怪物实例2 = new MonsterObject(value23, 地图实例9, int.MaxValue, new Point[1] { 皇宫左门坐标 }, forbidResurrection: true, 立即刷新: true);
                怪物实例2.CurrentDirection = GameDirection.DownLeft;
                怪物实例2.SurvivalTime = DateTime.MaxValue;
                左方宫门 = 怪物实例2;
                GameBuff.DataSheet.TryGetValue(22300, out var value24);
                沙城城门.AddBuff(value24.ID, 沙城城门);
                上方宫门.AddBuff(value24.ID, 上方宫门);
                下方宫门.AddBuff(value24.ID, 下方宫门);
                左方宫门.AddBuff(value24.ID, 左方宫门);
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 2)
            {
                if (SystemInfo.Info.OccupyGuild.V != null)
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                }
                foreach (KeyValuePair<DateTime, GuildInfo> item131 in SystemInfo.Info.申请行会.ToList())
                {
                    攻城行会.Add(SystemInfo.Info.OccupyGuild.V);
                    攻城行会.Add(item131.Value);
                    SystemInfo.Info.申请行会.Remove(item131.Key);
                    NetworkManager.SendAnnouncement($"获得攻占沙巴克资格行会【{item131.Value}】", rolling: true);
                }
                if (攻城行会.Count == 1 && SystemInfo.Info.OccupyGuild.V != null)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement($"只有沙巴克行会报名!沙巴克攻城战关闭,占领行会【{SystemInfo.Info.OccupyGuild.V}】", rolling: true);
                    return;
                }
                if (攻城行会.Count == 0)
                {
                    沙城节点 = 0;
                    沙城城门?.Despawn();
                    上方宫门?.Despawn();
                    下方宫门?.Despawn();
                    左方宫门?.Despawn();
                    NetworkManager.SendAnnouncement("今日无攻城行会,沙巴克攻城战关闭!", rolling: true);
                    return;
                }
                沙城节点 = 3;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 3)
            {
                沙城城门.移除Buff时处理(22300);
                下方宫门.移除Buff时处理(22300);
                上方宫门.移除Buff时处理(22300);
                左方宫门.移除Buff时处理(22300);
                NetworkManager.SendAnnouncement("沙巴克攻城战开始!勇士们为了荣誉战斗吧!", rolling: true);
                沙城节点 = 4;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
            {
                NetworkManager.SendAnnouncement("沙巴克城门已经被攻破", rolling: true);
                沙城城门.BirthMap = null;
                沙城节点 = 5;
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克开启 && 沙城节点 == 5)
            {
                皇宫随机区域 = 地图实例9.Areas.FirstOrDefault((MapArea O) => O.RegionName == "沙巴克-皇宫随机区域");
                bool flag23 = true;
                GuildInfo 行会数据23 = null;
                foreach (Point item132 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item133 in 地图实例9[item132])
                    {
                        if (!item133.Dead && item133 is PlayerObject 玩家实例88)
                        {
                            if (玩家实例88.Guild == null || !攻城行会.Contains(玩家实例88.Guild))
                            {
                                flag23 = false;
                                break;
                            }
                            if (行会数据23 == null)
                            {
                                行会数据23 = 玩家实例88.Guild;
                            }
                            else if (行会数据23 != 玩家实例88.Guild)
                            {
                                flag23 = false;
                                break;
                            }
                        }
                    }
                    if (!flag23)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据23) && SEngine.CurrentTime >= 沙城处理计时1 && 行会数据23 != null)
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据23;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据23.ID;
                    NetworkManager.Broadcast(同步占领行会);
                    沙城处理计时1 = SEngine.CurrentTime.AddSeconds(30.0);
                }
            }
            if (SEngine.CurrentTime.Hour == Config.沙巴克结束 && 沙城节点 == 5)
            {
                bool flag24 = true;
                GuildInfo 行会数据24 = null;
                foreach (Point item134 in 皇宫随机区域.RangeCoordinates)
                {
                    foreach (MapObject item135 in 地图实例9[item134])
                    {
                        if (!item135.Dead && item135 is PlayerObject 玩家实例89)
                        {
                            if (玩家实例89.Guild == null || !攻城行会.Contains(玩家实例89.Guild))
                            {
                                flag24 = false;
                                break;
                            }
                            if (行会数据24 == null)
                            {
                                行会数据24 = 玩家实例89.Guild;
                            }
                            else if (行会数据24 != 玩家实例89.Guild)
                            {
                                flag24 = false;
                                break;
                            }
                        }
                    }
                    if (!flag24)
                    {
                        break;
                    }
                }
                if (攻城行会.Contains(行会数据24))
                {
                    SystemInfo.Info.OccupyGuild.V = 行会数据24;
                    SystemInfo.Info.占领时间.V = SEngine.CurrentTime;
                    foreach (KeyValuePair<CharacterInfo, GuildRank> item136 in 行会数据24.Members)
                    {
                        item136.Key.攻沙日期.V = SEngine.CurrentTime;
                    }
                    NetworkManager.SendAnnouncement($"沙巴克攻城战已经结束, [{行会数据24}]成为新的沙巴克行会", rolling: true);
                    沙城节点 = 6;
                    同步占领行会 同步占领行会 = new 同步占领行会();
                    同步占领行会.NewGuildID = 行会数据24.ID;
                    NetworkManager.Broadcast(同步占领行会);
                }
            }
            沙城处理计时 = SEngine.CurrentTime.AddMilliseconds(1000.0);
        }
    }

    public static void Process()
    {
        try
        {
            foreach (KeyValuePair<int, MapObject> item in ActiveObjects)
            {
                item.Value?.Process();
            }
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
                    NetworkManager.SendAnnouncement("经验武斗场将在五分钟后开启, 想要参加的勇士请做好准备", rolling: true);
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

    public static void 开启地图()
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

        foreach (MapArea item in MapArea.DataSheet)
        {
            foreach (Map value6 in Maps.Values)
            {
                if (value6.MapID == item.MapID)
                {
                    if (item.RegionType == AreaType.Resurrection)
                    {
                        value6.ResurrectionArea = item;
                    }
                    if (item.RegionType == AreaType.红名区域)
                    {
                        value6.红名区域 = item;
                    }
                    if (item.RegionType == AreaType.Teleportation)
                    {
                        value6.TeleportationArea = item;
                    }
                    if (item.RegionType == AreaType.攻沙快捷)
                    {
                        value6.攻沙快捷 = item;
                    }
                    if (item.RegionType == AreaType.传送区域沙左)
                    {
                        value6.传送区域沙左 = item;
                    }
                    if (item.RegionType == AreaType.传送区域沙右)
                    {
                        value6.传送区域沙右 = item;
                    }
                    if (item.RegionType == AreaType.传送区域皇宫)
                    {
                        value6.传送区域皇宫 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔一)
                    {
                        value6.传送妖塔一 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔二)
                    {
                        value6.传送妖塔二 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔三)
                    {
                        value6.传送妖塔三 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔四)
                    {
                        value6.传送妖塔四 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔五)
                    {
                        value6.传送妖塔五 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔六)
                    {
                        value6.传送妖塔六 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔七)
                    {
                        value6.传送妖塔七 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔八)
                    {
                        value6.传送妖塔八 = item;
                    }
                    if (item.RegionType == AreaType.传送妖塔九)
                    {
                        value6.传送妖塔九 = item;
                    }
                    value6.Areas.Add(item);
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
        foreach (Map value10 in Maps.Values)
        {
            if (!value10.QuestMap)
            {
                foreach (MonsterSpawn item5 in value10.Spawns)
                {
                    if (item5.Spawns == null)
                    {
                        continue;
                    }
                    Point[] 出生范围 = item5.RangeCoordinates.ToArray();
                    MonsterSpawnInfo[] 刷新列表 = item5.Spawns;
                    MonsterSpawnInfo[] array = 刷新列表;
                    MonsterSpawnInfo[] array2 = array;
                    foreach (MonsterSpawnInfo 刷新信息 in array2)
                    {
                        if (MonsterInfo.DataSheet.TryGetValue(刷新信息.MonsterName, out var value))
                        {
                            SMain.添加怪物数据(value);
                            int 复活间隔 = 刷新信息.RevivalInterval * 60 * 1000;
                            for (int l = 0; l < 刷新信息.SpawnCount; l++)
                            {
                                new MonsterObject(value, value10, 复活间隔, 出生范围, forbidResurrection: false, 立即刷新: true);
                            }
                        }
                    }
                }
                foreach (MapGuard guard in value10.Guards)
                {
                    if (GuardInfo.DataSheet.TryGetValue(guard.GuardID, out var value2))
                    {
                        new GuardObject(value2, value10, guard.Direction, guard.Coordinates);
                    }
                }
            }
            else
            {
                value10.TotalFixedMonsters = (uint)value10.Spawns.Sum((MonsterSpawn O) => O.Spawns.Sum((MonsterSpawnInfo X) => X.SpawnCount));
            }
            SMain.添加地图数据(value10);
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
        攻城行会 = new HashSet<GuildInfo>();
    }
}
