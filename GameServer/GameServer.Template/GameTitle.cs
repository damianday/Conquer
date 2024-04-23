using System;
using System.Collections.Generic;

namespace GameServer.Template;

public sealed class GameTitle
{
    public static Dictionary<byte, GameTitle> DataSheet;

    public byte ID;
    public string Name;
    public int CombatPower;
    public int Duration;
    public Stats Stats;

    public static void LoadData()
    {
        DataSheet = new Dictionary<byte, GameTitle>();

        if (!DBAgent.X.Connected)
            return;

        try
        {
            var qstr = "SELECT * FROM GameTitles";
            using (var connection = DBAgent.X.DB.GetConnection())
            {
                using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                using var reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read() == true)
                    {
                        var obj = new GameTitle();

                        obj.ID = reader.GetByte("TitleID");
                        obj.Name = reader.GetString("Name");
                        obj.CombatPower = reader.GetInt32("CombatPower");
                        obj.Duration = reader.GetInt32("Duration");

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
                        obj.Stats[Stat.PhysicalAccuracy] = reader.GetInt32("PhysicalAccuracy");
                        obj.Stats[Stat.PhysicalAgility] = reader.GetInt32("PhysicalAgility");
                        obj.Stats[Stat.MagicEvade] = reader.GetInt32("MagicEvade");
                        obj.Stats[Stat.CriticalHitRate] = reader.GetInt32("CriticalHitRate");
                        obj.Stats[Stat.CriticalDamage] = reader.GetInt32("CriticalDamage");
                        obj.Stats[Stat.Luck] = reader.GetInt32("Luck");
                        obj.Stats[Stat.AttackSpeed] = reader.GetInt32("AttackSpeed");
                        obj.Stats[Stat.HealthRecovery] = reader.GetInt32("HealthRecovery");
                        obj.Stats[Stat.ManaRecovery] = reader.GetInt32("ManaRecovery");
                        obj.Stats[Stat.PoisonEvade] = reader.GetInt32("PoisonEvade");

                        DataSheet.Add(obj.ID, obj);
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
