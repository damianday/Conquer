using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GameServer.Template;

public sealed class VIPSystem
{
    public static Dictionary<int, VIPSystem> DataSheet = new Dictionary<int, VIPSystem>();

    public int VIPID;
    public int NeedVIPPoints;
    public int VIPIngot;
    public int VIPGoldCoin;
    public int 装备宝箱编号;
    public int 材料宝箱编号;
    public int 装备宝箱数量;
    public int 材料宝箱数量;
    public int VIPExperience;
    public int VIPRewardInterval;
    public int RequiredLevel;

    public static void LoadData()
    {
        DataSheet.Clear();

        if (Settings.Default.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM VIPSystem";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var obj = new VIPSystem
                            {
                                VIPID = reader.GetInt32("VIP编号"),
                                NeedVIPPoints = reader.GetInt32("VIP所需积分"),
                                VIPIngot = reader.GetInt32("VIP元宝受益"),
                                VIPGoldCoin = reader.GetInt32("VIP金币受益"),
                                装备宝箱编号 = reader.GetInt32("装备宝箱编号"),
                                材料宝箱编号 = reader.GetInt32("材料宝箱编号"),
                                装备宝箱数量 = reader.GetInt32("装备宝箱数量"),
                                材料宝箱数量 = reader.GetInt32("材料宝箱数量"),
                                VIPExperience = reader.GetInt32("VIP经验收益"),
                                VIPRewardInterval = reader.GetInt32("VIP收益间隔"),
                                RequiredLevel = reader.GetInt32("获得收益等级")
                            };
                            DataSheet.Add(obj.VIPID, obj);
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
            var path = Settings.Default.GameDataPath + "\\System\\VIPSystem";
            if (!Directory.Exists(path))
                return;

            using var reader = new StreamReader(path + "\\VIP系统数据.csv", Encoding.GetEncoding("GB18030"));
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    var obj = new VIPSystem
                    {
                        VIPID = csvReader.GetField<int>("VIP编号"),
                        NeedVIPPoints = csvReader.GetField<int>("VIP所需积分"),
                        VIPIngot = csvReader.GetField<int>("VIP元宝受益"),
                        VIPGoldCoin = csvReader.GetField<int>("VIP金币受益"),
                        装备宝箱编号 = csvReader.GetField<int>("装备宝箱编号"),
                        材料宝箱编号 = csvReader.GetField<int>("材料宝箱编号"),
                        装备宝箱数量 = csvReader.GetField<int>("装备宝箱数量"),
                        材料宝箱数量 = csvReader.GetField<int>("材料宝箱数量"),
                        VIPExperience = csvReader.GetField<int>("VIP经验收益"),
                        VIPRewardInterval = csvReader.GetField<int>("VIP收益间隔"),
                        RequiredLevel = csvReader.GetField<int>("获得收益等级")
                    };
                    DataSheet.Add(obj.VIPID, obj);
                }
            }
            catch (Exception ex)
            {
                SEngine.AddSystemLog(ex.Message);
            }
        }
    }
}
