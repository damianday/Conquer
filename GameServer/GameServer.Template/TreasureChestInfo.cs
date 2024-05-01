using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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

        if (Settings.Default.DBMethod == 1)
        {
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

        if (Settings.Default.DBMethod == 2)
        {
            var path = Settings.Default.GameDataPath + "\\System\\Items";
            if (!Directory.Exists(path))
                return;

            using var reader = new StreamReader(path + "\\TreasureChestData.csv", Encoding.GetEncoding("GB18030"));
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    TreasureChestInfo obj = new TreasureChestInfo
                    {
                        ChestID = csvReader.GetField<int>("ChestID"),
                        Item1ID = csvReader.GetField<int>("Item1ID"),
                        Item1Quantity = csvReader.GetField<int>("Item1Quantity"),
                        Item2ID = csvReader.GetField<int>("Item2ID"),
                        Item2Quantity = csvReader.GetField<int>("Item2Quantity"),
                        Item3ID = csvReader.GetField<int>("Item3ID"),
                        Item3Quantity = csvReader.GetField<int>("Item3Quantity"),
                        Item4ID = csvReader.GetField<int>("Item4ID"),
                        Item4Quantity = csvReader.GetField<int>("Item4Quantity"),
                        Item5ID = csvReader.GetField<int>("Item5ID"),
                        Item5Quantity = csvReader.GetField<int>("Item5Quantity"),
                        Item6ID = csvReader.GetField<int>("Item6ID"),
                        Item6Quantity = csvReader.GetField<int>("Item6Quantity")
                    };
                    DataSheet.Add(obj.ChestID, obj);
                }
            }
            catch (Exception ex)
            {
                SEngine.AddSystemLog(ex.Message);
            }
        }
    }
}
