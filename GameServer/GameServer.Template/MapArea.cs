using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GameServer.Template;

public class MapArea
{
    public static List<MapArea> DataSheet;

    public byte MapID;
    public string MapName;
    public Point Coordinates;
    public string RegionName;
    public int AreaRadius;
    public AreaType RegionType;
    public HashSet<Point> RangeCoordinates;

    private List<Point> m_ListCoords;

    public Point RandomCoords => RangeCoordsList[SEngine.Random.Next(RangeCoordsList.Count)];

    public List<Point> RangeCoordsList
    {
        get
        {
            if (m_ListCoords == null)
                m_ListCoords = RangeCoordinates.ToList();
            return m_ListCoords;
        }
    }

    public static void LoadData()
    {
        DataSheet = new List<MapArea>();

        var path = Config.GameDataPath + "\\System\\GameMap\\MapAreas\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<MapArea>(path);
        foreach (var obj in array)
            DataSheet.Add(obj);
    }
}
