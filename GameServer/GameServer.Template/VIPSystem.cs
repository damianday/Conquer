using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GameServer.Template;

public sealed class VIPSystem
{
    public static Dictionary<int, VIPSystem> DataSheet;

    public int VIPID;
    public int NeedVIPPoints;
    public int VIPIngot;
    public int VIPGoldCoin;
    public int 装备宝箱编号;
    public int 材料宝箱编号;
    public int 装备宝箱数量;
    public int 材料宝箱数量;
    public int VIPExperience;
    public int VIP收益间隔;
    public int 获得收益等级;

    public static void LoadData()
    {
        DataSheet = new Dictionary<int, VIPSystem>();

        var path = Config.GameDataPath + "\\System\\VIPSystem";
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
                    VIP收益间隔 = csvReader.GetField<int>("VIP收益间隔"),
                    获得收益等级 = csvReader.GetField<int>("获得收益等级")
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
