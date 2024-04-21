using System;
using System.Collections.Generic;

namespace GameServer.Template;

public sealed class TreasureChestInfo
{
    public static Dictionary<int, TreasureChestInfo> DataSheet;

    public int ChestID;
    public int Item1ID;
    public int Item1Quantity;
    public int Item2ID;
    public int Item2Quantity;
    public int Item3ID;
    public int Item3Quantity;
    public int Item4ID;
    public int Item4Quantity;
    public int Item5ID;
    public int Item5Quantity;
    public int Item6ID;
    public int Item6Quantity;


    public static void LoadData()
    {
        DataSheet = new Dictionary<int, TreasureChestInfo>();

        if (!DBAgent.X.Connected)
            return;

        try
        {
            var qstr = "SELECT * FROM TreasureChest";
            using (var connection = DBAgent.X.DB.GetConnection())
            {
                using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                using var reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read() == true)
                    {
                        var obj = new TreasureChestInfo
                        {
                            ChestID = reader.GetInt32("ChestID"),
                            Item1ID = reader.GetInt32("Item1ID"),
                            Item1Quantity = reader.GetInt32("Item1Quantity"),
                            Item2ID = reader.GetInt32("Item2ID"),
                            Item2Quantity = reader.GetInt32("Item2Quantity"),
                            Item3ID = reader.GetInt32("Item3ID"),
                            Item3Quantity = reader.GetInt32("Item3Quantity"),
                            Item4ID = reader.GetInt32("Item4ID"),
                            Item4Quantity = reader.GetInt32("Item4Quantity"),
                            Item5ID = reader.GetInt32("Item5ID"),
                            Item5Quantity = reader.GetInt32("Item5Quantity"),
                            Item6ID = reader.GetInt32("Item6ID"),
                            Item6Quantity = reader.GetInt32("Item6Quantity")
                        };
                        DataSheet.Add(obj.ChestID, obj);
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
