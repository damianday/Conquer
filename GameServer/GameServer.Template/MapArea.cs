using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameServer.Template;

public class MapArea
{
    public static List<MapArea> DataSheet;

    public byte MapID;
    public string MapName;
    public string RegionName;
    public Point Coordinates = Point.Empty;
    public int AreaRadius;
    public AreaType RegionType;

    public static void LoadData()
    {
        DataSheet = new List<MapArea>();

        if (Config.DBMethod == 0)
        {
            var path = Config.GameDataPath + "\\System\\GameMap\\MapAreas\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<MapArea>(path);
            foreach (var obj in array)
                DataSheet.Add(obj);
        }

        if (Config.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM MapRegions";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var region = new MapArea();
                            region.MapID = reader.GetByte("MapID");
                            region.MapName = reader.GetString("MapName");
                            region.RegionName = reader.GetString("RegionName");
                            region.Coordinates.X = reader.GetInt32("MapPosX");
                            region.Coordinates.Y = reader.GetInt32("MapPosY");
                            region.AreaRadius = reader.GetInt32("Radius");
                            region.RegionType = Enum.Parse<AreaType>(reader.GetString("RegionType"));

                            DataSheet.Add(region);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                SMain.AddSystemLog(err.ToString());
                return;
            }
        }
    }
}
