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
    private static int ObjectTableIndex;

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

    public static HashSet<Map> ReplicaMaps;

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

    public static MonsterObject WorldBoss; // Conqueror's Dragon

    public static Map SandCityMap;

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


    public static byte SandCityStage;

    private static DateTime NotificationTime;
    private static DateTime MonsterWorldBossZenTime;
    private static DateTime MonsterBossZenTime;

    public static HashSet<GuildInfo> SiegeGuilds;

    private static void ProcessSandCity()
    {
        var day = DateTime.UtcNow.DayOfWeek;

        if (SEngine.CurrentTime.Hour + 2 >= Settings.Default.沙巴克开启 && Settings.Default.沙巴克每周攻沙时间 == 0)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Settings.Default.沙巴克开启 && day == DayOfWeek.Monday && Settings.Default.沙巴克每周攻沙时间 == 1)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Settings.Default.沙巴克开启 && day == DayOfWeek.Tuesday && Settings.Default.沙巴克每周攻沙时间 == 2)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Settings.Default.沙巴克开启 && day == DayOfWeek.Wednesday && Settings.Default.沙巴克每周攻沙时间 == 3)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Settings.Default.沙巴克开启 && day == DayOfWeek.Thursday && Settings.Default.沙巴克每周攻沙时间 == 4)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Settings.Default.沙巴克开启 && day == DayOfWeek.Friday && Settings.Default.沙巴克每周攻沙时间 == 5)
        {
            ProcessSabakWar();
        }
        else if (SEngine.CurrentTime.Hour + 2 >= Settings.Default.沙巴克开启 && day == DayOfWeek.Saturday && Settings.Default.沙巴克每周攻沙时间 == 6)
        {
            ProcessSabakWar();
        }
        else
        {
            if (SEngine.CurrentTime.Hour + 2 < Settings.Default.沙巴克开启 || day != DayOfWeek.Sunday || Settings.Default.沙巴克每周攻沙时间 != 7)
                return;

            ProcessSabakWar();
        }
    }

    private static void ProcessSabakWar()
    {
        static GuildInfo FindGuild(Map map)
        {
            GuildInfo guild = null;

            foreach (var player in Players.Values)
            {
                if (player.Dead) continue;

                if (!Compute.InRange(皇宫随机区域.Coordinates, player.CurrentPosition, 皇宫随机区域.AreaRadius)) continue;

                if (player.Guild == null || !SiegeGuilds.Contains(player.Guild))
                    break;
                if (guild == null)
                    guild = player.Guild;
                else if (guild != player.Guild)
                    break;
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

        if (SEngine.CurrentTime.Hour + 1 < Settings.Default.沙巴克开启)
        {
            SandCityStage = 0;
        }

        if (SEngine.CurrentTime.Hour + 1 == Settings.Default.沙巴克开启 && SEngine.CurrentTime.Minute == 50 && SandCityStage == 0)
        {
            NetworkManager.SendAnnouncement("The Siege of Sabak will begin in ten minutes, so be prepared", true);
            SandCityStage = 1;
        }

        if (Settings.Default.沙巴克停止开关 == 1)
        {
            SandCityStage = 0;
            Settings.Default.沙巴克停止开关 = 0;

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

        if (SEngine.CurrentTime.Hour == Settings.Default.沙巴克开启 && Settings.Default.沙巴克重置系统 == 1)
        {
            SystemInfo.Info.OccupyGuild.V = null;
            Settings.Default.沙巴克重置系统 = 0;
            NetworkManager.SendAnnouncement("Sabak reset, about to start the siege, please get ready", true);
            SandCityStage = 1;
        }

        if (SandCityStage == 1)
        {
            SandCityStage = 2;

            if (MonsterInfo.DataSheet.TryGetValue("沙巴克城门", out var value))
            {
                沙城城门 = new MonsterObject(value, map, int.MaxValue, 沙城城门坐标, 1, true, true);
                沙城城门.CurrentDirection = GameDirection.UpRight;
                沙城城门.SurvivalTime = DateTime.MaxValue;
            }

            if (MonsterInfo.DataSheet.TryGetValue("沙巴克宫门", out var value2))
            {
                上方宫门 = new MonsterObject(value2, map, int.MaxValue, 皇宫上门坐标, 1, true, true);
                上方宫门.CurrentDirection = GameDirection.DownRight;
                上方宫门.SurvivalTime = DateTime.MaxValue;

                下方宫门 = new MonsterObject(value2, map, int.MaxValue, 皇宫下门坐标, 1, true, true);
                下方宫门.CurrentDirection = GameDirection.DownRight;
                下方宫门.SurvivalTime = DateTime.MaxValue;

                左方宫门 = new MonsterObject(value2, map, int.MaxValue, 皇宫左门坐标, 1, true, true);
                左方宫门.CurrentDirection = GameDirection.DownLeft;
                左方宫门.SurvivalTime = DateTime.MaxValue;
            }

            if (GameBuff.DataSheet.TryGetValue(22300, out var buff))
            {
                if (沙城城门 != null) 沙城城门.AddBuff(buff.ID, 沙城城门);
                if (上方宫门 != null) 上方宫门.AddBuff(buff.ID, 上方宫门);
                if (下方宫门 != null) 下方宫门.AddBuff(buff.ID, 下方宫门);
                if (左方宫门 != null) 左方宫门.AddBuff(buff.ID, 左方宫门);
            }
        }

        if (SEngine.CurrentTime.Hour == Settings.Default.沙巴克开启 && SandCityStage == 2)
        {
            if (SystemInfo.Info.OccupyGuild.V != null)
            {
                SiegeGuilds.Add(SystemInfo.Info.OccupyGuild.V);
                NetworkManager.SendAnnouncement($"Gain the qualification guild to capture Sabak【{SystemInfo.Info.OccupyGuild.V}】", true);
            }
            foreach (KeyValuePair<DateTime, GuildInfo> item12 in SystemInfo.Info.GuildApplications.ToList())
            {
                SiegeGuilds.Add(SystemInfo.Info.OccupyGuild.V);
                SiegeGuilds.Add(item12.Value);
                SystemInfo.Info.GuildApplications.Remove(item12.Key);
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

        if (SEngine.CurrentTime.Hour == Settings.Default.沙巴克开启 && SandCityStage == 3)
        {
            沙城城门.RemoveBuffEx(22300);
            下方宫门.RemoveBuffEx(22300);
            上方宫门.RemoveBuffEx(22300);
            左方宫门.RemoveBuffEx(22300);
            NetworkManager.SendAnnouncement("The Siege of Sabak begins! Warriors fight for honor!", true);
            SandCityStage = 4;
        }

        if (SEngine.CurrentTime.Hour == Settings.Default.沙巴克开启 && SandCityStage == 4 && 沙城城门.Dead && 沙城城门.BirthMap != null)
        {
            NetworkManager.SendAnnouncement("The gates of Sabak have been breached", true);
            沙城城门.BirthMap = null;
            SandCityStage = 5;
        }

        if (SEngine.CurrentTime.Hour == Settings.Default.沙巴克开启 && SandCityStage == 5)
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

        if (SEngine.CurrentTime.Hour == Settings.Default.沙巴克结束 && SandCityStage == 5)
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

            if (ObjectTableIndex >= SecondaryObjects.Count)
            {
                ObjectTableIndex = 0;
                SecondaryObjects = BackupObjects;
                BackupObjects = new List<MapObject>();
            }

            for (int i = 0; i < 100; i++)
            {
                if (ObjectTableIndex >= SecondaryObjects.Count)
                    break;

                if (SecondaryObjects[ObjectTableIndex].SecondaryObject)
                {
                    SecondaryObjects[ObjectTableIndex].Process();
                    BackupObjects.Add(SecondaryObjects[ObjectTableIndex]);
                }
                ObjectTableIndex++;
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

            if (SEngine.CurrentTime.Minute == 55 && SEngine.CurrentTime.Hour != NotificationTime.Hour)
            {
                if (SEngine.CurrentTime.Hour + 1 == Settings.Default.武斗场时间一 || SEngine.CurrentTime.Hour + 1 == Settings.Default.武斗场时间二)
                {
                    NetworkManager.SendAnnouncement("The Experience Arena will open in five minutes, so be prepared if you want to participate", rolling: true);
                }
                NotificationTime = SEngine.CurrentTime;
            }

            if (SEngine.CurrentTime > MonsterBossZenTime)
            {
                foreach (var spawn in MonsterBossTimedZen.DataSheet)
                {
                    if (SEngine.CurrentTime.Minute == spawn.Minute && SEngine.CurrentTime.Hour == spawn.Hour)
                    {
                        if (MonsterInfo.DataSheet.TryGetValue(spawn.MonsterName, out var moni))
                        {
                            var map = GetMap(spawn.MapID);
                            var mon = new MonsterObject(moni, map, int.MaxValue, spawn.Coordinates, 1, true, true)
                            {
                                CurrentDirection = GameDirection.UpRight,
                                SurvivalTime = SEngine.CurrentTime.AddMinutes(spawn.SurvivalTime)
                            };
                            NetworkManager.SendAnnouncement("World Boss [ " + moni.MonsterName + " ] is here. " + map.MapInfo.MapName + ", Players who wish to participate should prepare themselves.", rolling: true);
                        }
                    }
                }

                MonsterBossZenTime = SEngine.CurrentTime.AddSeconds(15.0);
            }

            if (Settings.Default.CurrentVersion >= 3 && SEngine.CurrentTime.Minute == Settings.Default.WorldBossTimeMinute && SEngine.CurrentTime.Hour != MonsterWorldBossZenTime.Hour && SEngine.CurrentTime.Second == 1)
            {
                if (SEngine.CurrentTime.Hour == Settings.Default.WorldBossTimeHour && MonsterInfo.DataSheet.TryGetValue(Settings.Default.WorldBossName, out var value))
                {
                    Map map = GetMap(Settings.Default.WorldBossMapID);
                    WorldBoss = new MonsterObject(value, map, int.MaxValue, new Point(Settings.Default.WorldBossMapPosX, Settings.Default.WorldBossMapPosY), 1,
                        forbidResurrection: true, refreshNow: true)
                    {
                        CurrentDirection = GameDirection.UpRight,
                        SurvivalTime = DateTime.MaxValue
                    };
                    NetworkManager.SendAnnouncement("The World Boss [ " + Settings.Default.WorldBossName + " ]has arrived at the Secret Treasure Square. Warriors who want to participate please be prepared", rolling: true);
                }
                MonsterWorldBossZenTime = SEngine.CurrentTime;
            }

            foreach (Map map in ReplicaMaps)
            {
                if (map.ReplicaClosed)
                    ReplicateClosingMaps.Enqueue(map);
                else
                    map.Process();
            }

            while (!ReplicateClosingMaps.IsEmpty)
            {
                if (ReplicateClosingMaps.TryDequeue(out var map))
                    ReplicaMaps.Remove(map);
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
        ReplicaMaps = new HashSet<Map>();
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

        MonsterBossZenTime = SEngine.CurrentTime;

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
                map.Cells = new HashSet<MapObject>[map.MapSize.Width, map.MapSize.Height];
                for (var x = 0; x < map.MapSize.Width; x++)
                {
                    for (var y = 0; y < map.MapSize.Height; y++)
                    {
                        map.Cells[x, y] = new HashSet<MapObject>();
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

                    foreach (MonsterSpawnInfo spawni in spawn.Spawns)
                    {
                        if (MonsterInfo.DataSheet.TryGetValue(spawni.MonsterName, out var moni))
                        {
                            SMain.AddMonsterData(moni);
                            int duration = spawni.RevivalInterval * 60 * 1000;
                            for (int i = 0; i < spawni.SpawnCount; i++)
                            {
                                new MonsterObject(moni, map, duration, spawn.Coordinates, spawn.AreaRadius, false, true);
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
                map.TotalFixedMonsters = (uint)map.Spawns.Sum(x => x.Spawns.Sum(x => x.SpawnCount));
            }

            map.MakeStoneMines();

            SMain.AddMapData(map);
        }
    }

    public static void RemoveItems()
    {
        foreach (var item in Items.Values)
            item.Item?.Remove();

        foreach (var kvp in GameStore.DataSheet)
        {
            foreach (var item in kvp.Value.AvailableItems)
                item.Remove();
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
