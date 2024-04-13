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
    public int EffectiveTime;
    public Stats Stats;

    public static void LoadData()
    {
        DataSheet = new Dictionary<byte, GameTitle>();

        var path = Config.GameDataPath + "\\System\\Items\\GameTitle\\";
        if (Directory.Exists(path) && Config.称号属性切换 == 0)
        {
            var array = Serializer.Deserialize<GameTitle>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.ID, obj);
        }

        if (!Directory.Exists(path) || Config.称号属性切换 != 1)
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
                obj.EffectiveTime = csvReader.GetField<int>("有效时间");

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
                obj.Stats[Stat.怪物伤害] = csvReader.GetField<int>("怪物伤害");
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
