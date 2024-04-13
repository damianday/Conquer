using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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

        var path = Config.GameDataPath + "\\System\\Mounts\\MountBeast\\";
        if (Directory.Exists(path) && Config.坐骑属性切换 == 0)
        {
            var array = Serializer.Deserialize<MountBeast>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.MountID, obj);
        }
        if (!Directory.Exists(path) || Config.坐骑属性切换 != 1)
        {
            return;
        }

        using var reader = new StreamReader(path + "\\坐骑御兽.csv", Encoding.GetEncoding("GB18030"));
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Read();
        csvReader.ReadHeader();
        try
        {
            while (csvReader.Read())
            {
                var obj = new MountBeast();
                obj.MountID = csvReader.GetField<ushort>("坐骑编号");
                obj.Name = csvReader.GetField<string>("坐骑名字");

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

                DataSheet.Add(obj.MountID, obj);
            }
        }
        catch (Exception ex)
        {
            SEngine.AddSystemLog(ex.Message);
        }
    }
}
