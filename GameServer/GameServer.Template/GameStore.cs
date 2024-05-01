using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameServer.Database;

namespace GameServer.Template;

public sealed class GameStoreItem
{
    public int ItemID;          // 商品编号
    public int Quantity;        // 单位数量
    public int CurrencyModel;   // 货币类型
    public int Price;           // 商品价格
}

public sealed class GameStore
{
    public static byte[] StoreMemory;
    public static int StoreChecksum;
    public static int StoreCount;
    public static int SellItemID;

    public static Dictionary<int, GameStore> DataSheet;

    public int StoreID;
    public string Name;
    public SaleType StoreType = SaleType.All;
    public List<GameStoreItem> Products = new List<GameStoreItem>();
    public SortedSet<ItemInfo> AvailableItems = new SortedSet<ItemInfo>(new PurchaseComparer());

    public static void LoadData()
    {
        DataSheet = new Dictionary<int, GameStore>();

        if (Settings.DBMethod == 0)
        {
            var path = Settings.GameDataPath + "\\System\\Items\\GameStore\\"; // "\\System\\物品数据\\游戏商店\\"
            if (!Directory.Exists(path))
                return;

            var array = Serializer.Deserialize<GameStore>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.StoreID, obj);
        }

        if (Settings.DBMethod == 1)
        {
            if (!DBAgent.X.Connected)
                return;

            try
            {
                var qstr = "SELECT * FROM GameStores";
                using (var connection = DBAgent.X.DB.GetConnection())
                {
                    using var command = DBAgent.X.DB.GetCommand(connection, qstr);

                    using var reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read() == true)
                        {
                            var store = new GameStore();
                            store.StoreID = reader.GetInt32("StoreID");
                            store.Name = reader.GetString("Name");
                            store.StoreType = (SaleType)reader.GetInt32("StoreType");
                            
                            DataSheet.Add(store.StoreID, store);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                SMain.AddSystemLog(err.ToString());
                return;
            }

            try
            {
                foreach (var store in DataSheet)
                {
                    var qstr = "SELECT * FROM GameStoreProduct WHERE StoreID=@StoreID";
                    using (var connection = DBAgent.X.DB.GetConnection())
                    {
                        using var command = DBAgent.X.DB.GetCommand(connection, qstr);
                        command.Parameters.AddWithValue("@StoreID", store.Value.StoreID);

                        using var reader = command.ExecuteReader();
                        if (reader != null)
                        {
                            while (reader.Read() == true)
                            {
                                var product = new GameStoreItem();
                                product.ItemID = reader.GetInt32("ItemID");
                                product.Quantity = reader.GetInt32("Quantity");
                                product.CurrencyModel = reader.GetInt32("CurrencyModel");
                                product.Price = reader.GetInt32("Price");

                                store.Value.Products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                SMain.AddSystemLog(err.ToString());
                return;
            }
        }

        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        var stores = DataSheet.Values.OrderBy(x => x.StoreID);
        foreach (var store in stores)
        {
            foreach (var product in store.Products)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(store.Name);
                writer.Write(store.StoreID);
                writer.Write(bytes);
                writer.Write(new byte[64 - bytes.Length]);
                writer.Write(product.ItemID);
                writer.Write(product.Quantity);
                writer.Write(product.CurrencyModel);
                writer.Write(product.Price);
                writer.Write(-1);
                writer.Write(0);
                writer.Write(-1);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write((int)store.StoreType);
                writer.Write(0);
                writer.Write(0);
                writer.Write((ushort)0);
                writer.Write(-1);
                writer.Write(-1);
                StoreCount++;
            }
        }

        byte[] data = ms.ToArray();
        StoreMemory = Serializer.CompressBytes(data);
        StoreChecksum = 0;
        foreach (byte b in StoreMemory)
            StoreChecksum += b;
    }

    public bool BuyItem(ItemInfo item)
    {
        return AvailableItems.Remove(item);
    }

    public void SellItem(ItemInfo item)
    {
        item.PurchaseID = ++SellItemID;
        if (AvailableItems.Add(item) && AvailableItems.Count > 50)
        {
            var last = AvailableItems.Last();
            AvailableItems.Remove(last);
            last.Remove();
        }
    }
}
