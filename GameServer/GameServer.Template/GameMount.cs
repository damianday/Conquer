using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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

        if (Config.DBMethod == 0)
        {
            var path = Config.GameDataPath + "\\System\\Mounts\\RidingMounts";
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<GameMount>(path);
            foreach (var mount in array)
            {
                mount.Stats = new Stats();
                mount.Stats[Stat.WalkSpeed] = mount.SpeedModificationRate / 500;
                mount.Stats[Stat.RunSpeed] = mount.SpeedModificationRate / 500;

                DataSheet.Add(mount.ID, mount);
            }
        }

        if (Config.DBMethod == 1)
        {
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

        if (Config.DBMethod == 2)
        {
            var path = Config.GameDataPath + "\\System\\Mounts\\RidingMounts";
            if (!Directory.Exists(path))
                return;

            using var reader = new StreamReader(path + "\\骑马坐骑.csv", Encoding.GetEncoding("GB18030"));
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    GameMount mount = new GameMount
                    {
                        ID = csvReader.GetField<ushort>("坐骑编号"),
                        Name = csvReader.GetField<string>("坐骑名字"),
                        AuraID = csvReader.GetField<int>("AuraID"),
                        MountPower = csvReader.GetField<short>("MountPower"),
                        BuffID = csvReader.GetField<ushort>("Buff编号"),
                        Quality = csvReader.GetField<byte>("品质"),
                        RequiredLevel = csvReader.GetField<byte>("等级限制"),
                        SpeedModificationRate = csvReader.GetField<int>("速度修改率"),
                        HitUnmountRate = csvReader.GetField<int>("HitUnmountRate")
                    };

                    mount.Stats = new Stats();
                    mount.Stats[Stat.WalkSpeed] = mount.SpeedModificationRate / 500;
                    mount.Stats[Stat.RunSpeed] = mount.SpeedModificationRate / 500;

                    DataSheet.Add(mount.ID, mount);
                }
            }
            catch (Exception ex)
            {
                SEngine.AddSystemLog(ex.Message);
            }
        }
    }
}
