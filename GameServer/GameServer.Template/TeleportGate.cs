using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameServer.Template;

public class TeleportGate
{
    public static List<TeleportGate> DataSheet;

    public byte GateID;
    public byte MapID;
    public byte ToMapID;
    public string GateName;
    public string MapName;
    public string ToMapName;
    public string MapFileName;
    public string ToMapFileName;
    public Point Coordinates;
    public Point ToCoordinates;

    public static void LoadData()
    {
        DataSheet = new List<TeleportGate>();

        var path = Config.GameDataPath + "\\System\\GameMap\\TeleportGates\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<TeleportGate>(path);
        foreach (var obj in array)
            DataSheet.Add(obj);
    }
}
