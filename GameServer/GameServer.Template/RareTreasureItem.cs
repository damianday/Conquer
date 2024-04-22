using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameServer.Template;

public sealed class RareTreasureItem
{
    public static byte[] Buffer;
    public static int Checksum;
    public static int TreasureCount;
    public static Dictionary<int, RareTreasureItem> DataSheet;

    public int ItemID;
    public int UnitCount;
    public byte Type;
    public byte Label;
    public byte AdditionalParam;
    public int OriginalPrice;
    public int CurrentPrice;
    public byte BuyBound;
    public int UID;
    public int 珍宝阁排版;
    public DateTime StartTime;
    public DateTime EndTime;
    public byte 事件;
    public byte 类别;

    public static void LoadData()
    {
        DataSheet = new Dictionary<int, RareTreasureItem>();

        if (!DBAgent.X.Connected)
            return;

        try
        {
            var qstr = "SELECT * FROM Treasure";
            using (var connection = DBAgent.X.DB.GetConnection())
            {
                using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                using var reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read() == true)
                    {
                        var treasure = new RareTreasureItem
                        {
                            ItemID = reader.GetInt32("ItemID"),
                            UnitCount = reader.GetInt32("UnitCount"),
                            Type = reader.GetByte("Type"),
                            Label = reader.GetByte("Label"),
                            AdditionalParam = reader.GetByte("AdditionalParam"),
                            OriginalPrice = reader.GetInt32("OriginalPrice"),
                            CurrentPrice = reader.GetInt32("CurrentPrice"),
                            BuyBound = reader.GetByte("BuyBound"),
                            UID = reader.GetInt32("UID"),
                            珍宝阁排版 = reader.GetInt32("珍宝阁排版"),
                            类别 = reader.GetByte("类别"),
                            事件 = reader.GetByte("事件")
                        };
                        DataSheet.Add(treasure.ItemID, treasure);
                    }
                }
            }
        }
        catch (Exception err)
        {
            SMain.AddSystemLog(err.ToString());
            return;
        }

        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        foreach (var treasure in DataSheet.Values.OrderBy(x => x.ItemID))
        {
            writer.Write(treasure.ItemID);
            writer.Write(treasure.UnitCount);
            writer.Write(treasure.Type);
            writer.Write(treasure.Label);
            writer.Write(treasure.AdditionalParam); 
            writer.Write(treasure.OriginalPrice);
            writer.Write(treasure.CurrentPrice);
            writer.Write(new byte[8]);
            writer.Write(treasure.UID);
            writer.Write(treasure.珍宝阁排版);
            writer.Write(new byte[10]);
            writer.Write(Compute.TimeSeconds(treasure.StartTime));
            writer.Write(Compute.TimeSeconds(treasure.EndTime));
            writer.Write(new byte[12]);
            writer.Write(treasure.事件);
            writer.Write(treasure.类别);
        }
        TreasureCount = DataSheet.Count;
        Buffer = ms.ToArray();
        Checksum = 0;

        foreach (byte b in Buffer)
            Checksum += b;
    }
}
