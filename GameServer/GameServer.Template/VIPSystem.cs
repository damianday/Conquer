using System;
using System.Collections.Generic;

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
                            VIP收益间隔 = reader.GetInt32("VIP收益间隔"),
                            获得收益等级 = reader.GetInt32("获得收益等级")
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
}
