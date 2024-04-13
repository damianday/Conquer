using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameServer.Template;

public class MonsterSpawn
{
    public static HashSet<MonsterSpawn> DataSheet;

    public byte MapID;
    public string MapName;
    public Point Coordinates;
    public string RegionName;
    public int AreaRadius;
    public MonsterSpawnInfo[] Spawns;
    public HashSet<Point> RangeCoordinates;

    public static void LoadData()
    {
        DataSheet = new HashSet<MonsterSpawn>();

        var path = Config.GameDataPath + "\\System\\GameMap\\Monsters\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<MonsterSpawn>(path);
        foreach (var obj in array)
            DataSheet.Add(obj);
    }
}
