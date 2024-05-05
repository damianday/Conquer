using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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

        if (Settings.Default.DBMethod == 0)
        {
            var path = Settings.Default.GameDataPath + "\\System\\Items\\GameTitle\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<GameTitle>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.ID, obj);
        }

        if (Settings.Default.DBMethod == 1)
        {
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
                            obj.Stats[Stat.MonsterDamage] = reader.GetInt32("MonsterDamage");
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

        if (Settings.Default.DBMethod == 2)
        {
            var path = Settings.Default.GameDataPath + "\\System\\Items\\GameTitle\\";
            if (!Directory.Exists(path))
                return;

            var reader = new StreamReader(path + "\\称号模块.csv", Encoding.GetEncoding("GB18030"));
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    GameTitle obj = new GameTitle();

                    obj.ID = csvReader.GetField<byte>("称号编号");
                    obj.Name = csvReader.GetField<string>("称号名字");
                    obj.CombatPower = csvReader.GetField<int>("称号战力");
                    obj.Duration = csvReader.GetField<int>("有效时间");

                    obj.Stats = new Stats();
                    obj.Stats[Stat.MinDef] = csvReader.GetField<int>("最小防御");
                    obj.Stats[Stat.MaxDef] = csvReader.GetField<int>("最大防御");
                    obj.Stats[Stat.MinMCDef] = csvReader.GetField<int>("最小魔防");
                    obj.Stats[Stat.MaxMCDef] = csvReader.GetField<int>("最大魔防");
                    obj.Stats[Stat.MinDC] = csvReader.GetField<int>("最小攻击");
                    obj.Stats[Stat.MaxDC] = csvReader.GetField<int>("最大攻击");
                    obj.Stats[Stat.MinMC] = csvReader.GetField<int>("最小魔法");
                    obj.Stats[Stat.MaxMC] = csvReader.GetField<int>("最大魔法");
                    obj.Stats[Stat.MinSC] = csvReader.GetField<int>("最小道术");
                    obj.Stats[Stat.MaxSC] = csvReader.GetField<int>("最大道术");
                    obj.Stats[Stat.MinNC] = csvReader.GetField<int>("最小刺术");
                    obj.Stats[Stat.MaxNC] = csvReader.GetField<int>("最大刺术");
                    obj.Stats[Stat.MinBC] = csvReader.GetField<int>("最小弓术");
                    obj.Stats[Stat.MaxBC] = csvReader.GetField<int>("最大弓术");
                    obj.Stats[Stat.MaxHP] = csvReader.GetField<int>("最大体力");
                    obj.Stats[Stat.MaxMP] = csvReader.GetField<int>("最大魔力");
                    obj.Stats[Stat.MinHC] = csvReader.GetField<int>("最小神圣伤害");
                    obj.Stats[Stat.MaxHC] = csvReader.GetField<int>("最大神圣伤害");
                    obj.Stats[Stat.MonsterDamage] = csvReader.GetField<int>("怪物伤害");
                    obj.Stats[Stat.PhysicalAccuracy] = csvReader.GetField<int>("物理准确");
                    obj.Stats[Stat.PhysicalAgility] = csvReader.GetField<int>("物理敏捷");
                    obj.Stats[Stat.MagicEvade] = csvReader.GetField<int>("魔法闪避");
                    obj.Stats[Stat.CriticalHitRate] = csvReader.GetField<int>("暴击概率");
                    obj.Stats[Stat.CriticalDamage] = csvReader.GetField<int>("暴击伤害");
                    obj.Stats[Stat.Luck] = csvReader.GetField<int>("幸运等级");
                    obj.Stats[Stat.AttackSpeed] = csvReader.GetField<int>("攻击速度");
                    obj.Stats[Stat.HealthRecovery] = csvReader.GetField<int>("体力恢复");
                    obj.Stats[Stat.ManaRecovery] = csvReader.GetField<int>("魔力恢复");
                    obj.Stats[Stat.PoisonEvade] = csvReader.GetField<int>("中毒躲避");

                    DataSheet.Add(obj.ID, obj);
                }
            }
            catch (Exception ex)
            {
                SEngine.AddSystemLog(ex.Message);
            }
        }
    }
}
