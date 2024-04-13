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
    public byte LevelLimit;
    public int SpeedModificationRate;
    public int HitUnmountRate;
    public Stats Stats;

    public static void LoadData()
    {
        DataSheet = new Dictionary<uint, GameMount>();

        var path = Config.GameDataPath + "\\System\\Mounts\\RidingMounts";
        if (Directory.Exists(path) && Config.坐骑骑乘切换 == 0)
        {
            var array = Serializer.Deserialize<GameMount>(path);
            foreach (var mount in array)
            {
                mount.Stats = new Stats();
                mount.Stats[Stat.WalkSpeed] = mount.SpeedModificationRate / 500;
                mount.Stats[Stat.RunSpeed] = mount.SpeedModificationRate / 500;

                DataSheet.Add(mount.ID, mount);
            }
        }
        if (!Directory.Exists(path) || Config.坐骑骑乘切换 != 1)
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
                    LevelLimit = csvReader.GetField<byte>("等级限制"),
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
