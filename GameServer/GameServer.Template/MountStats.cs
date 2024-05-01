using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GameServer.Template;

public sealed class MountStats
{
    public static IDictionary<uint, MountStats> DataSheet;

    public ushort MountID;
    public string Name;
    public Stats Stats;

    public static void LoadData()
    {
        DataSheet = new Dictionary<uint, MountStats>();

        if (Settings.Default.DBMethod == 0)
        {
            var path = Settings.Default.GameDataPath + "\\System\\Mounts\\MountBeast\\";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<MountStats>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.MountID, obj);
        }

        if (Settings.Default.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM MountStats";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var obj = new MountStats();
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

        if (Settings.Default.DBMethod == 2)
        {
            var path = Settings.Default.GameDataPath + "\\System\\Mounts\\MountBeast\\";
            if (!Directory.Exists(path))
                return;
            using var reader = new StreamReader(path + "\\坐骑御兽.csv", Encoding.GetEncoding("GB18030"));
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    var obj = new MountStats();
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
}
