using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GameServer.Template;

public class MonsterSpawn
{
    public static HashSet<MonsterSpawn> DataSheet;

    public byte MapID;
    public Point Coordinates = Point.Empty;
    public string RegionName;
    public int AreaRadius;
    public List<MonsterSpawnInfo> Spawns = new List<MonsterSpawnInfo>();

    public static void LoadData()
    {
        DataSheet = new HashSet<MonsterSpawn>();

        if (Settings.DBMethod == 0)
        {
            var path = Settings.GameDataPath + "\\System\\GameMap\\Monsters\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<MonsterSpawn>(path);
            foreach (var obj in array)
                DataSheet.Add(obj);
        }

        if (Settings.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM MonsterZen";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var mapid = reader.GetByte("MapID");
                            var xx = reader.GetInt32("MapPosX");
                            var yy = reader.GetInt32("MapPosY");
                            var radius = reader.GetInt32("Radius");

                            var spawn = DataSheet.FirstOrDefault(x => x.MapID == mapid &&
                                x.Coordinates.X == xx && x.Coordinates.Y == yy &&
                                x.AreaRadius == radius);
                            if (spawn == null)
                            {
                                spawn = new MonsterSpawn();
                                spawn.MapID = mapid;
                                spawn.Coordinates.X = xx;
                                spawn.Coordinates.Y = yy;
                                spawn.RegionName = reader.GetString("RegionName");
                                spawn.AreaRadius = radius;
                            }

                            var spawni = new MonsterSpawnInfo();
                            spawni.MonsterName = reader.GetString("MonsterName");
                            spawni.SpawnCount = reader.GetInt32("Amount");
                            spawni.RevivalInterval = reader.GetInt32("RevivalInterval");

                            spawn.Spawns.Add(spawni);

                            DataSheet.Add(spawn);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                SMain.AddSystemLog(err.ToString());
            }
        }
    }
}
