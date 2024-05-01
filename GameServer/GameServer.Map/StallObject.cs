using System.Collections.Generic;
using System.IO;
using GameServer.Database;

namespace GameServer.Map;

public sealed class StallObject
{
    public byte Status;
    public string Name;

    public Dictionary<ItemInfo, int> Quantities;
    public Dictionary<ItemInfo, int> Prices;
    public Dictionary<byte, ItemInfo> Items;

    public StallObject()
    {
        Status = 1;
        Quantities = new Dictionary<ItemInfo, int>();
        Prices = new Dictionary<ItemInfo, int>();
        Items = new Dictionary<byte, ItemInfo>();
    }

    public long TotalPrice
    {
        get
        {
            long price = 0L;
            foreach (var item in Items.Values)
                price += (long)Quantities[item] * (long)Prices[item];
            return price;
        }
    }

    public byte[] 摊位描述()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write((byte)Items.Count);
        foreach (var kvp in Items)
        {
            writer.Write(kvp.Key);
            writer.Write(Prices[kvp.Value]);
            writer.Write(0);
            writer.Write(0);
            if (kvp.Value.PersistType == PersistentItemType.装备 && Settings.CurrentVersion >= 1)
            {
                EquipmentInfo 装备数据 = kvp.Value as EquipmentInfo;
                writer.Write(装备数据.ToArray());
            }
            else
            {
                writer.Write(kvp.Value.ToArray(Quantities[kvp.Value]));
            }
        }
        return ms.ToArray();
    }
}
