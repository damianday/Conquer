using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GameServer.Template;

public sealed class ItemCrafting
{
    public static Dictionary<int, ItemCrafting> DataSheet;

    public int RecipeItemID;
    public int MakeItemID;
    public int DoubleFee;
    public int GoldCost;
    public int MaterialItem1ID;
    public int MaterialItem1Quantity;
    public int MaterialItem2ID;
    public int MaterialItem2Quantity;
    public int MaterialItem3ID;
    public int MaterialItem3Quantity;
    public int MaterialItem4ID;
    public int MaterialItem4Quantity;
    public int MaterialItem5ID;
    public int MaterialItem5Quantity;
    public int MaterialItem6ID;
    public int MaterialItem6Quantity;
    public int Broadcast;


    public static void LoadData()
    {
        DataSheet = new Dictionary<int, ItemCrafting>();

        if (Config.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM Crafting";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var obj = new ItemCrafting
                            {
                                RecipeItemID = reader.GetInt32("RecipeItemID"),
                                MakeItemID = reader.GetInt32("MakeItemID"),
                                DoubleFee = reader.GetInt32("DoubleFee"),
                                GoldCost = reader.GetInt32("GoldCost"),
                                MaterialItem1ID = reader.GetInt32("MaterialItem1ID"),
                                MaterialItem1Quantity = reader.GetInt32("MaterialItem1Quantity"),
                                MaterialItem2ID = reader.GetInt32("MaterialItem2ID"),
                                MaterialItem2Quantity = reader.GetInt32("MaterialItem2Quantity"),
                                MaterialItem3ID = reader.GetInt32("MaterialItem3ID"),
                                MaterialItem3Quantity = reader.GetInt32("MaterialItem3Quantity"),
                                MaterialItem4ID = reader.GetInt32("MaterialItem4ID"),
                                MaterialItem4Quantity = reader.GetInt32("MaterialItem4Quantity"),
                                MaterialItem5ID = reader.GetInt32("MaterialItem5ID"),
                                MaterialItem5Quantity = reader.GetInt32("MaterialItem5Quantity"),
                                MaterialItem6ID = reader.GetInt32("MaterialItem6ID"),
                                MaterialItem6Quantity = reader.GetInt32("MaterialItem6Quantity"),
                                Broadcast = reader.GetInt32("Broadcast")
                            };
                            DataSheet.Add(obj.RecipeItemID, obj);
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
            var path = Config.GameDataPath + "\\System\\Items\\Crafting";
            if (!Directory.Exists(path))
                return;

            var reader = new StreamReader(path + "\\CraftingData.csv", Encoding.GetEncoding("GB18030"));
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    var obj = new ItemCrafting
                    {
                        RecipeItemID = csvReader.GetField<int>("RecipeItemID"),
                        MakeItemID = csvReader.GetField<int>("MakeItemID"),
                        DoubleFee = csvReader.GetField<int>("DoubleFee"),
                        GoldCost = csvReader.GetField<int>("GoldCost"),
                        MaterialItem1ID = csvReader.GetField<int>("MaterialItem1ID"),
                        MaterialItem1Quantity = csvReader.GetField<int>("MaterialItem1Quantity"),
                        MaterialItem2ID = csvReader.GetField<int>("MaterialItem2ID"),
                        MaterialItem2Quantity = csvReader.GetField<int>("MaterialItem2Quantity"),
                        MaterialItem3ID = csvReader.GetField<int>("MaterialItem3ID"),
                        MaterialItem3Quantity = csvReader.GetField<int>("MaterialItem3Quantity"),
                        MaterialItem4ID = csvReader.GetField<int>("MaterialItem4ID"),
                        MaterialItem4Quantity = csvReader.GetField<int>("MaterialItem4Quantity"),
                        MaterialItem5ID = csvReader.GetField<int>("MaterialItem5ID"),
                        MaterialItem5Quantity = csvReader.GetField<int>("MaterialItem5Quantity"),
                        MaterialItem6ID = csvReader.GetField<int>("MaterialItem6ID"),
                        MaterialItem6Quantity = csvReader.GetField<int>("MaterialItem6Quantity"),
                        Broadcast = csvReader.GetField<int>("Broadcast")
                    };
                    DataSheet.Add(obj.RecipeItemID, obj);
                }
            }
            catch (Exception ex)
            {
                SEngine.AddSystemLog(ex.Message);
            }
        }

    }
}
