using System;
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
    public Point Coordinates = Point.Empty;
    public Point ToCoordinates = Point.Empty;

    public static void LoadData()
    {
        DataSheet = new List<TeleportGate>();

        if (Settings.DBMethod == 0)
        {
            var path = Settings.GameDataPath + "\\System\\GameMap\\TeleportGates\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<TeleportGate>(path);
            foreach (var obj in array)
                DataSheet.Add(obj);
        }

        if (Settings.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM Portals";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var gate = new TeleportGate();
                            gate.GateID = reader.GetByte("GateID");
                            gate.MapID = reader.GetByte("MapID");
                            gate.ToMapID = reader.GetByte("ToMapID");
                            gate.GateName = reader.GetString("GateName");
                            gate.MapName = reader.GetString("MapName");
                            gate.ToMapName = reader.GetString("ToMapName");
                            gate.MapFileName = reader.GetString("MapFileName");
                            gate.ToMapFileName = reader.GetString("ToMapFileName");
                            gate.Coordinates.X = reader.GetInt32("MapPosX");
                            gate.Coordinates.Y = reader.GetInt32("MapPosY");
                            gate.ToCoordinates.X = reader.GetInt32("ToMapPosX");
                            gate.ToCoordinates.Y = reader.GetInt32("ToMapPosY");
                            DataSheet.Add(gate);
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
