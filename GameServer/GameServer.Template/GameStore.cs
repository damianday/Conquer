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
    public SaleType StoreType;
    public List<GameStoreItem> Products;
    public SortedSet<ItemInfo> AvailableItems = new SortedSet<ItemInfo>(new PurchaseComparer());

    public static void LoadData()
    {
        DataSheet = new Dictionary<int, GameStore>();

        var path = Config.GameDataPath + "\\System\\Items\\GameStore\\"; // "\\System\\物品数据\\游戏商店\\"
        if (Directory.Exists(path))
        {
            var array = Serializer.Deserialize<GameStore>(path);
            foreach (var obj in array)
                DataSheet.Add(obj.StoreID, obj);
        }

        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        var stores = DataSheet.Values.ToList().OrderBy(x => x.StoreID);
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
