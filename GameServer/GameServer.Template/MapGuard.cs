using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameServer.Template;

public sealed class MapGuard
{
    public static HashSet<MapGuard> DataSheet;

    public ushort GuardID;
    public byte MapID;
    public string MapName;
    public Point Coordinates;
    public GameDirection Direction;
    public string RegionName;

    public static void LoadData()
    {
        DataSheet = new HashSet<MapGuard>();

        string path = Config.GameDataPath + "\\System\\GameMap\\Guards\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<MapGuard>(path);
        foreach (var obj in array)
            DataSheet.Add(obj);
    }
}
