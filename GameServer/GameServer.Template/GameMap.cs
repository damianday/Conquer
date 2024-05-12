using System;
using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public sealed class GameMap
{
    public static Dictionary<byte, GameMap> DataSheet = new Dictionary<byte, GameMap>();

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
        DataSheet.Clear();

        if (Settings.Default.DBMethod == 0)
        {
            var path = Settings.Default.GameDataPath + "\\System\\GameMap\\Maps";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<GameMap>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.MapID, obj);
        }

        if (Settings.Default.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM Maps";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var map = new GameMap
                            {
                                MapID = reader.GetByte("MapID"),
                                MapName = reader.GetString("MapName"),
                                MapFile = reader.GetString("MapFile"),
                                TerrainFile = reader.GetString("TerrainFile"),
                                LimitPlayers = reader.GetInt32("LimitPlayers"),
                                MinLevel = reader.GetByte("MinLevel"),
                                LimitInstances = reader.GetByte("LimitInstances"),
                                NoReconnect = reader.GetBoolean("NoReconnect"),
                                NoReconnectMapID = reader.GetByte("NoReconnectMapID"),
                                QuestMap = reader.GetBoolean("QuestMap"),
                                MineMap = reader.GetByte("MineMap")
                            };
                            DataSheet.Add(map.MapID, map);
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

    public override string ToString() => MapName;
}
