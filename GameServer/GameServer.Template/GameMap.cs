using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public sealed class GameMap
{
    public static Dictionary<byte, GameMap> DataSheet;

    public byte MapID;
    public string MapName;
    public string MapFile;
    public string TerrainFile;
    public int LimitPlayers;
    public byte MinLevel;
    public byte LimitInstances;
    public bool NoReconnect;
    public byte NoReconnectMapID;
    public bool QuestMap;
    public byte MineMap;

    public static void LoadData()
    {
        DataSheet = new Dictionary<byte, GameMap>();

        var path = Config.GameDataPath + "\\System\\GameMap\\Maps";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<GameMap>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.MapID, obj);
    }

    public override string ToString() => MapName;
}
