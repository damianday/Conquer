using System;
using System.Collections.Generic;

namespace GameServer.Template;

public sealed class GameMount
{
    public static IDictionary<uint, GameMount> DataSheet;

    public ushort ID;
    public string Name;
    public int AuraID;
    public short MountPower;
    public ushort BuffID;
    public byte Quality;
    public byte RequiredLevel;
    public int SpeedModificationRate;
    public int HitUnmountRate;
    public Stats Stats;

    public static void LoadData()
    {
        DataSheet = new Dictionary<uint, GameMount>();

        if (!DBAgent.X.Connected)
            return;

        try
        {
            var qstr = "SELECT * FROM Mounts";
            using (var connection = DBAgent.X.DB.GetConnection())
            {
                using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                using var reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read() == true)
                    {
                        GameMount mount = new GameMount
                        {
                            ID = reader.GetUInt16("MountID"),
                            Name = reader.GetString("Name"),
                            AuraID = reader.GetInt32("AuraID"),
                            MountPower = reader.GetInt16("MountPower"),
                            BuffID = reader.GetUInt16("BuffID"),
                            Quality = reader.GetByte("Quality"),
                            RequiredLevel = reader.GetByte("RequiredLevel"),
                            SpeedModificationRate = reader.GetInt32("SpeedModificationRate"),
                            HitUnmountRate = reader.GetInt32("HitUnmountRate")
                        };

                        mount.Stats = new Stats();
                        mount.Stats[Stat.WalkSpeed] = mount.SpeedModificationRate / 500;
                        mount.Stats[Stat.RunSpeed] = mount.SpeedModificationRate / 500;

                        DataSheet.Add(mount.ID, mount);
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
