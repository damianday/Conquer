using System.Collections.Generic;
using GameServer.Database;

namespace GameServer.Template;

public sealed class PurchaseComparer : IComparer<ItemInfo>
{
    public int Compare(ItemInfo a, ItemInfo b)
    {
        return b.PurchaseID.CompareTo(a.PurchaseID);
    }
}
