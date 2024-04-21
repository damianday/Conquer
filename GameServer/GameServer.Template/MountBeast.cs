using System;
using System.Collections.Generic;

namespace GameServer.Template;

public sealed class MountBeast
{
    public static IDictionary<uint, MountBeast> DataSheet;

    public ushort MountID;
    public string Name;
    public Stats Stats;

    public static void LoadData()
    {
        DataSheet = new Dictionary<uint, MountBeast>();

        if (!DBAgent.X.Connected)
            return;

        try
        {
            var qstr = "SELECT * FROM MountBeast";
            using (var connection = DBAgent.X.DB.GetConnection())
            {
                using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                using var reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read() == true)
                    {
                        var obj = new MountBeast();
                        obj.MountID = reader.GetUInt16("MountID");
                        obj.Name = reader.GetString("Name");

                        obj.Stats = new Stats();
                        obj.Stats[Stat.MinDef] = reader.GetInt32("MinDef");
                        obj.Stats[Stat.MaxDef] = reader.GetInt32("MaxDef");
                        obj.Stats[Stat.MinMCDef] = reader.GetInt32("MinMCDef");
                        obj.Stats[Stat.MaxMCDef] = reader.GetInt32("MaxMCDef");
                        obj.Stats[Stat.MinDC] = reader.GetInt32("MinDC");
                        obj.Stats[Stat.MaxDC] = reader.GetInt32("MaxDC");
                        obj.Stats[Stat.MinMC] = reader.GetInt32("MinMC");
                        obj.Stats[Stat.MaxMC] = reader.GetInt32("MaxMC");
                        obj.Stats[Stat.MinSC] = reader.GetInt32("MinSC");
                        obj.Stats[Stat.MaxSC] = reader.GetInt32("MaxSC");
                        obj.Stats[Stat.MinNC] = reader.GetInt32("MinNC");
                        obj.Stats[Stat.MaxNC] = reader.GetInt32("MaxNC");
                        obj.Stats[Stat.MinBC] = reader.GetInt32("MinBC");
                        obj.Stats[Stat.MaxBC] = reader.GetInt32("MaxBC");
                        obj.Stats[Stat.MaxHP] = reader.GetInt32("MaxHP");
                        obj.Stats[Stat.MaxMP] = reader.GetInt32("MaxMP");
                        obj.Stats[Stat.MinHC] = reader.GetInt32("MinHC");
                        obj.Stats[Stat.MaxHC] = reader.GetInt32("MaxHC");
                        obj.Stats[Stat.怪物伤害] = reader.GetInt32("怪物伤害");

                        DataSheet.Add(obj.MountID, obj);
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
