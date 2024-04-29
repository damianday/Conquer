using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GameServer.Template;

public class MonsterBossTimedZen
{
    public static HashSet<MonsterBossTimedZen> DataSheet;

    public byte MapID;
    public Point Coordinates = Point.Empty;
    public byte Hour;
    public byte Minute;
    public ushort MonsterID;
    public string MonsterName;
    public int SurvivalTime;

    public static void LoadData()
    {
        DataSheet = new HashSet<MonsterBossTimedZen>();

        if (Config.DBMethod == 0)
        {
            var path = Config.GameDataPath + "\\System\\GameMap\\MonsterBossTimed\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<MonsterBossTimedZen>(path);
            foreach (var obj in array)
                DataSheet.Add(obj);
        }

        if (Config.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM MonsterBossTimedZen";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var zen = new MonsterBossTimedZen();
                            zen.MapID = reader.GetByte("MapID");
                            zen.Coordinates.X = reader.GetInt32("MapPosX");
                            zen.Coordinates.Y = reader.GetInt32("MapPosY");
                            zen.Hour = reader.GetByte("Hour");
                            zen.Minute = reader.GetByte("Minute");
                            zen.MonsterID = reader.GetUInt16("MonsterID");
                            zen.MonsterName = reader.GetString("MonsterName");
                            zen.SurvivalTime = reader.GetInt32("SurvivalTime");
                            DataSheet.Add(zen);
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
