using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using GameServer.Template;
using GameServer.Networking;

using GamePackets;
using GamePackets.Server;

namespace GameServer.Map;

public sealed class Map
{
    public readonly int RouteID;
    public readonly GameMap MapInfo;

    public uint TotalFixedMonsters;
    public uint TotalSurvivingMonsters;
    public uint TotalAmountMonsterResurrected;
    public long TotalAmountMonsterDrops;
    public long TotalAmountGoldDrops;
    public bool ReplicaClosed;
    public byte 副本节点;
    public byte 九层副本节点;
    public GuardObject 副本守卫;
    public DateTime ProcessTime;
    public int 刷怪记录;
    public List<MonsterSpawn> Respawns;
    public HashSet<MapObject>[,] Cells;
    public Terrain Terrain;

    public MapArea ResurrectionArea;
    public MapArea RedNameArea;
    public MapArea TeleportationArea;
    public MapArea 攻沙快捷;
    public MapArea 传送区域沙左;
    public MapArea 传送区域沙右;
    public MapArea 传送区域皇宫;
    public MapArea DemonTower1Area;
    public MapArea DemonTower2Area;
    public MapArea DemonTower3Area;
    public MapArea DemonTower4Area;
    public MapArea DemonTower5Area;
    public MapArea DemonTower6Area;
    public MapArea DemonTower7Area;
    public MapArea DemonTower8Area;
    public MapArea DemonTower9Area;

    public HashSet<MapArea> Areas;
    public HashSet<MonsterSpawn> Spawns;
    public HashSet<MapGuard> Guards;
    public HashSet<PlayerObject> Players;
    public HashSet<PetObject> Pets;
    public HashSet<ItemObject> Items;
    public HashSet<MapObject> Objects;
    public Dictionary<byte, TeleportGate> TeleportGates;

    public byte MapStatus
    {
        get
        {
            if (Players.Count < 200)
                return 1;
            if (Players.Count < 500)
                return 2;
            return 3;
        }
    }

    public int MapID => MapInfo.MapID;
    public byte MinLevel => MapInfo.MinLevel;
    public byte LimitInstances => 1;
    public bool NoReconnect => MapInfo.NoReconnect;
    public byte NoReconnectMapID => MapInfo.NoReconnectMapID;
    public bool QuestMap => MapInfo.QuestMap;

    public Point StartPoint => Terrain.StartPoint;
    public Point EndPoint => Terrain.EndPoint;
    public Point MapSize => Terrain.MapSize;

    public HashSet<MapObject> this[Point point]
    {
        get
        {
            if (!ValidPoint(point))
                return new HashSet<MapObject>();

            if (Cells[point.X - StartPoint.X, point.Y - StartPoint.Y] == null)
                return Cells[point.X - StartPoint.X, point.Y - StartPoint.Y] = new HashSet<MapObject>();

            return Cells[point.X - StartPoint.X, point.Y - StartPoint.Y];
        }
    }

    public Map(GameMap info, int 路线编号 = 1)
    {
        Areas = new HashSet<MapArea>();
        Spawns = new HashSet<MonsterSpawn>();
        Guards = new HashSet<MapGuard>();
        Players = new HashSet<PlayerObject>();
        Pets = new HashSet<PetObject>();
        Items = new HashSet<ItemObject>();
        Objects = new HashSet<MapObject>();
        TeleportGates = new Dictionary<byte, TeleportGate>();
        this.MapInfo = info;
        this.RouteID = 路线编号;
    }

    public void Process()
    {
        if (MapID != 80)
        {
            return;
        }
        if (Players.Count == 0)
        {
            副本节点 = 110;
        }
        else if (副本节点 <= 5)
        {
            if (SEngine.CurrentTime > ProcessTime)
            {
                地图公告($"怪物将在{30 - 副本节点 * 5}秒后刷新, 请做好准备");
                副本节点++;
                ProcessTime = SEngine.CurrentTime.AddSeconds(5.0);
            }
        }
        else if (副本节点 <= 5 + Respawns.Count)
        {
            if (副本守卫.Dead)
            {
                副本节点 = 100;
                ProcessTime = SEngine.CurrentTime;
            }
            else if (SEngine.CurrentTime > ProcessTime)
            {
                int num = 副本节点 - 6;
                MonsterSpawn spawn = Respawns[num];
                int num2 = 刷怪记录 >> 16;
                int num3 = 刷怪记录 & 0xFFFF;
                MonsterSpawnInfo 刷新信息 = spawn.Spawns[num2];
                if (刷怪记录 == 0)
                {
                    地图公告($"第{num + 1}波怪物已经出现, 请注意防守");
                }
                if (MonsterInfo.DataSheet.TryGetValue(刷新信息.MonsterName, out var moni))
                {
                    new MonsterObject(moni, this, int.MaxValue, new Point[1]
                    {
                        new Point(995, 283)
                    }, forbidResurrection: true, 立即刷新: true).SurvivalTime = SEngine.CurrentTime.AddMinutes(30.0);
                }
                if (++num3 >= 刷新信息.SpawnCount)
                {
                    num2++;
                    num3 = 0;
                }
                if (num2 >= spawn.Spawns.Length)
                {
                    副本节点++;
                    刷怪记录 = 0;
                    ProcessTime = SEngine.CurrentTime.AddSeconds(60.0);
                }
                else
                {
                    刷怪记录 = (num2 << 16) + num3;
                    ProcessTime = SEngine.CurrentTime.AddSeconds(2.0);
                }
            }
        }
        else if (副本节点 == 6 + Respawns.Count)
        {
            if (副本守卫.Dead)
            {
                副本节点 = 100;
                ProcessTime = SEngine.CurrentTime;
            }
            else if (TotalSurvivingMonsters == 0)
            {
                地图公告("所有怪物都已被击退, 大厅将在30秒后关闭");
                副本节点 = 110;
                ProcessTime = SEngine.CurrentTime.AddSeconds(30.0);
            }
        }
        else if (副本节点 <= 109)
        {
            if (SEngine.CurrentTime > ProcessTime)
            {
                地图公告("守卫已经死亡, 大厅即将关闭");
                副本节点 += 2;
                ProcessTime = SEngine.CurrentTime.AddSeconds(2.0);
            }
        }
        else
        {
            if (副本节点 < 110 || !(SEngine.CurrentTime > ProcessTime))
            {
                return;
            }
            foreach (var player in Players)
            {
                if (player.Dead)
                    player.Resurrect();
                else
                    player.Teleport(MapManager.GetMap(player.RespawnMapIndex), AreaType.Resurrection);
            }
            foreach (var pet in Pets)
            {
                if (pet.Dead)
                    pet.Despawn();
                else
                    pet.PetRecall();
            }
            foreach (var item in Items)
                item.DestroyItem();
            foreach (var obj in Objects)
                obj.Despawn();
            ReplicaClosed = true;
        }
    }

    public void AddObject(MapObject obj)
    {
        switch (obj.ObjectType)
        {
            case GameObjectType.Item: Items.Add(obj as ItemObject); break;
            case GameObjectType.Pet: Pets.Add(obj as PetObject); break;
            case GameObjectType.Player: Players.Add(obj as PlayerObject); break;
            default: Objects.Add(obj); break;
        }
    }

    public void RemoveObject(MapObject obj)
    {
        switch (obj.ObjectType)
        {
            case GameObjectType.Item: Items.Remove(obj as ItemObject); break;
            case GameObjectType.Pet: Pets.Remove(obj as PetObject); break;
            case GameObjectType.Player: Players.Remove(obj as PlayerObject); break;
            default: Objects.Remove(obj); break;
        }
    }

    public void 地图公告(string 内容)
    {
        if (Players.Count == 0)
            return;

        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(0);
        writer.Write(2415919107u);
        writer.Write(3);
        writer.Write(0);
        writer.Write(Encoding.UTF8.GetBytes(内容 + "\0"));
        byte[] buffer = ms.ToArray();
        foreach (var player in Players)
        {
            player.Enqueue(new SystemMessagePacket
            {
                Description = buffer
            });
        }
    }

    public override string ToString()
    {
        return MapInfo.ToString();
    }

    public Point RandomPosition(AreaType region)
    {
        Point result = region switch
        {
            AreaType.Resurrection => ResurrectionArea.RandomCoords,
            AreaType.攻沙快捷 => 攻沙快捷.RandomCoords,
            AreaType.传送区域沙左 => 传送区域沙左.RandomCoords,
            AreaType.传送区域沙右 => 传送区域沙右.RandomCoords,
            AreaType.传送区域皇宫 => 传送区域皇宫.RandomCoords,
            AreaType.RedName => RedNameArea.RandomCoords,
            AreaType.Teleportation => TeleportationArea.RandomCoords,
            AreaType.DemonTower1 => DemonTower1Area.RandomCoords,
            AreaType.DemonTower2 => DemonTower2Area.RandomCoords,
            AreaType.DemonTower3 => DemonTower3Area.RandomCoords,
            AreaType.DemonTower4 => DemonTower4Area.RandomCoords,
            AreaType.DemonTower5 => DemonTower5Area.RandomCoords,
            AreaType.DemonTower6 => DemonTower6Area.RandomCoords,
            AreaType.DemonTower7 => DemonTower7Area.RandomCoords,
            AreaType.DemonTower8 => DemonTower8Area.RandomCoords,
            AreaType.DemonTower9 => DemonTower9Area.RandomCoords,
            AreaType.Random => Areas.FirstOrDefault(x => x.RegionType == AreaType.Random)?.RandomCoords ?? default(Point),
            _ => default(Point),
        };

        return result;
    }

    public Point 随机传送(Point point)
    {
        foreach (var area in Areas)
        {
            if (area.RangeCoordinates.Contains(point) && area.RegionType == AreaType.Random)
                return area.RandomCoords;
        }
        return Point.Empty;
    }

    public bool ValidPoint(Point point)
    {
        return point.X >= StartPoint.X && point.Y >= StartPoint.Y && point.X < EndPoint.X && point.Y < EndPoint.Y;
    }

    public bool IsBlocking(Point point)
    {
        if (IsSafeArea(point))
            return false;

        foreach (var obj in this[point])
        {
            if (obj.Blocking)
                return true;
        }
        return false;
    }

    public int BlockingCount(Point point)
    {
        int count = 0;
        foreach (var obj in this[point])
        {
            if (obj.Blocking)
                count++;
        }
        return count;
    }

    public bool ValidTerrain(Point point)
    {
        if (ValidPoint(point))
            return (Terrain[point] & 0x10000000) == 268435456;
        return false;
    }

    public bool CanMove(Point point)
    {
        if (ValidTerrain(point))
            return !IsBlocking(point);
        return false;
    }

    public ushort GetTerrainHeight(Point point)
    {
        if (ValidPoint(point))
            return (ushort)((Terrain[point] & 0xFFFF) - 30);
        return 0;
    }

    public bool IsTerrainBlocked(Point start, Point end)
    {
        var distance = Compute.GetDistance(start, end);
        for (var i = 1; i <= distance; i++)
        {
            if (!ValidTerrain(Compute.GetFrontPosition(start, end, i)))
                return false;
        }
        return true;
    }

    public bool 自由区内(Point point)
    {
        if (ValidPoint(point))
            return (Terrain[point] & 0x20000) == 131072;
        return false;
    }

    public bool IsSafeArea(Point point)
    {
        if (ValidPoint(point))
        {
            if ((Terrain[point] & 0x40000) != 262144)
                return (Terrain[point] & 0x100000) == 1048576;
            return true;
        }
        return false;
    }

    public bool IsStallArea(Point point)
    {
        if (ValidPoint(point))
            return (Terrain[point] & 0x100000) == 1048576;
        return false;
    }

    public bool 掉落装备(Point point, bool redName)
    {
        if (MapManager.SandCityStage >= 2 && (MapID == 152 || MapID == 178) && Config.沙巴克爆装备开关 == 0)
            return false;

        if (ValidPoint(point))
        {
            if ((Terrain[point] & 0x400000) == 4194304)
                return true;
            if ((Terrain[point] & 0x800000) == 8388608 && redName)
                return true;
        }
        return false;
    }
}
