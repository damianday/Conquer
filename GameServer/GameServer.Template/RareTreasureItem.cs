using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

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

    //public byte 职业限制;
    //public int 商品编号;
    //public int 热销排名;
    //public byte 活动商品;
    //public byte 绑定商品;

    public static void LoadData()
    {
        DataSheet = new Dictionary<int, RareTreasureItem>();

        var path = Config.GameDataPath + "\\System\\Items\\Treasures\\";
        if (Directory.Exists(path) && Config.珍宝模块切换 == 0)
        {
            var array = Serializer.Deserialize<RareTreasureItem>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.ItemID, obj);
        }

        if (Directory.Exists(path) && Config.珍宝模块切换 == 1)
        {
            using var reader = new StreamReader(path + "\\珍宝数据.csv", Encoding.GetEncoding("GB18030"));
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Read();
            csvReader.ReadHeader();
            try
            {
                while (csvReader.Read())
                {
                    var treasure = new RareTreasureItem
                    {
                        ItemID = csvReader.GetField<int>("物品编号"),
                        UnitCount = csvReader.GetField<int>("单位数量"),
                        Type = csvReader.GetField<byte>("商品分类"),
                        Label = csvReader.GetField<byte>("商品标签"),
                        AdditionalParam = csvReader.GetField<byte>("补充参数"),
                        OriginalPrice = csvReader.GetField<int>("商品原价"),
                        CurrentPrice = csvReader.GetField<int>("商品现价"),
                        BuyBound = csvReader.GetField<byte>("买入绑定"),
                        UID = csvReader.GetField<int>("Uid"),
                        珍宝阁排版 = csvReader.GetField<int>("珍宝阁排版"),
                        类别 = csvReader.GetField<byte>("类别"),
                        事件 = csvReader.GetField<byte>("事件")
                    };
                    DataSheet.Add(treasure.ItemID, treasure);
                }
            }
            catch (Exception ex)
            {
                SEngine.AddSystemLog(ex.Message);
            }
        }

        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        foreach (RareTreasureItem treasure in
            (from X in DataSheet.Values.ToList()
             orderby X.ItemID
             select X).ToList())
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
