using System;
using System.Collections.Generic;
using System.IO;
using GameServer.Template;

namespace GameServer.Database;

public class ItemInfo : DBObject
{
    public const byte Version = 15;

    public readonly DataMonitor<GameItem> Item;
    public readonly DataMonitor<DateTime> CreatedDate;
    public readonly DataMonitor<CharacterInfo> Owner;
    public readonly DataMonitor<int> Dura;
    public readonly DataMonitor<int> MaxDura;
    public readonly DataMonitor<byte> Grid;
    public readonly DataMonitor<byte> Location;
    public int PurchaseID;
    public GameItem Info => Item.V;
    public SaleType StoreType => Info.StoreType;
    public ItemType Type => Info.Type;
    public PersistentItemType PersistType => Info.PersistType;
    public GameObjectRace NeedRace => Info.NeedRace;
    public GameObjectGender NeedGender => Info.NeedGender;
    public string Name => Info.Name;
    public int NeedLevel => Info.NeedLevel;
    public int ID => Info.ID;

    public int Weight
    {
        get
        {
            if (PersistType != PersistentItemType.Stack)
                return Info.Weight;
            return Dura.V * Info.Weight;
        }
    }

    public int SalePrice
    {
        get
        {
            switch (Info.PersistType)
            {
                default:
                    return 0;
                case PersistentItemType.None:
                    return 1;
                case PersistentItemType.Equipment:
                    {
                        EquipmentInfo 装备数据2 = this as EquipmentInfo;
                        EquipmentItem 游戏装备 = Info as EquipmentItem;
                        int v3 = 装备数据2.Dura.V;
                        int num2 = 游戏装备.MaxDura * 1000;
                        int num3 = 游戏装备.SalePrice;
                        int num4 = Math.Max((sbyte)0, 装备数据2.Luck.V);
                        int num5 = 装备数据2.DCPower.V * 100 + 装备数据2.MCPower.V * 100 + 装备数据2.SCPower.V * 100 + 装备数据2.NCPower.V * 100 + 装备数据2.BCPower.V * 100;
                        int num6 = 0;
                        foreach (InscriptionSkill value in 装备数据2.InscriptionSkills.Values)
                        {
                            if (value != null)
                            {
                                num6++;
                            }
                        }
                        int num7 = 0;
                        foreach (RandomStats item in 装备数据2.RandomStats)
                        {
                            num7 += item.CombatBonus * 100;
                        }
                        int num8 = 0;
                        using (IEnumerator<GameItem> enumerator3 = 装备数据2.镶嵌灵石.Values.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                switch (enumerator3.Current.Name)
                                {
                                    case "驭朱灵石8级":
                                    case "精绿灵石8级":
                                    case "韧紫灵石8级":
                                    case "抵御幻彩灵石8级":
                                    case "进击幻彩灵石8级":
                                    case "盈绿灵石8级":
                                    case "狂热幻彩灵石8级":
                                    case "透蓝灵石8级":
                                    case "守阳灵石8级":
                                    case "新阳灵石8级":
                                    case "命朱灵石8级":
                                    case "蔚蓝灵石8级":
                                    case "赤褐灵石8级":
                                    case "橙黄灵石8级":
                                    case "纯紫灵石8级":
                                    case "深灰灵石8级":
                                        num8 += 8000;
                                        break;
                                    case "精绿灵石5级":
                                    case "新阳灵石5级":
                                    case "命朱灵石5级":
                                    case "蔚蓝灵石5级":
                                    case "橙黄灵石5级":
                                    case "进击幻彩灵石5级":
                                    case "深灰灵石5级":
                                    case "盈绿灵石5级":
                                    case "透蓝灵石5级":
                                    case "韧紫灵石5级":
                                    case "抵御幻彩灵石5级":
                                    case "驭朱灵石5级":
                                    case "赤褐灵石5级":
                                    case "守阳灵石5级":
                                    case "狂热幻彩灵石5级":
                                    case "纯紫灵石5级":
                                        num8 += 5000;
                                        break;
                                    case "精绿灵石2级":
                                    case "蔚蓝灵石2级":
                                    case "驭朱灵石2级":
                                    case "橙黄灵石2级":
                                    case "守阳灵石2级":
                                    case "纯紫灵石2级":
                                    case "透蓝灵石2级":
                                    case "抵御幻彩灵石2级":
                                    case "命朱灵石2级":
                                    case "深灰灵石2级":
                                    case "赤褐灵石2级":
                                    case "新阳灵石2级":
                                    case "进击幻彩灵石2级":
                                    case "狂热幻彩灵石2级":
                                    case "盈绿灵石2级":
                                    case "韧紫灵石2级":
                                        num8 += 2000;
                                        break;
                                    case "抵御幻彩灵石7级":
                                    case "命朱灵石7级":
                                    case "赤褐灵石7级":
                                    case "狂热幻彩灵石7级":
                                    case "精绿灵石7级":
                                    case "纯紫灵石7级":
                                    case "韧紫灵石7级":
                                    case "驭朱灵石7级":
                                    case "深灰灵石7级":
                                    case "盈绿灵石7级":
                                    case "新阳灵石7级":
                                    case "蔚蓝灵石7级":
                                    case "橙黄灵石7级":
                                    case "守阳灵石7级":
                                    case "进击幻彩灵石7级":
                                    case "透蓝灵石7级":
                                        num8 += 7000;
                                        break;
                                    case "精绿灵石9级":
                                    case "驭朱灵石9级":
                                    case "蔚蓝灵石9级":
                                    case "橙黄灵石9级":
                                    case "抵御幻彩灵石9级":
                                    case "透蓝灵石9级":
                                    case "纯紫灵石9级":
                                    case "命朱灵石9级":
                                    case "赤褐灵石9级":
                                    case "深灰灵石9级":
                                    case "守阳灵石9级":
                                    case "新阳灵石9级":
                                    case "盈绿灵石9级":
                                    case "进击幻彩灵石9级":
                                    case "狂热幻彩灵石9级":
                                    case "韧紫灵石9级":
                                        num8 += 9000;
                                        break;
                                    case "驭朱灵石4级":
                                    case "深灰灵石4级":
                                    case "新阳灵石4级":
                                    case "盈绿灵石4级":
                                    case "蔚蓝灵石4级":
                                    case "命朱灵石4级":
                                    case "橙黄灵石4级":
                                    case "进击幻彩灵石4级":
                                    case "抵御幻彩灵石4级":
                                    case "透蓝灵石4级":
                                    case "守阳灵石4级":
                                    case "精绿灵石4级":
                                    case "赤褐灵石4级":
                                    case "纯紫灵石4级":
                                    case "韧紫灵石4级":
                                    case "狂热幻彩灵石4级":
                                        num8 += 4000;
                                        break;
                                    case "透蓝灵石6级":
                                    case "抵御幻彩灵石6级":
                                    case "命朱灵石6级":
                                    case "盈绿灵石6级":
                                    case "深灰灵石6级":
                                    case "蔚蓝灵石6级":
                                    case "进击幻彩灵石6级":
                                    case "橙黄灵石6级":
                                    case "赤褐灵石6级":
                                    case "驭朱灵石6级":
                                    case "精绿灵石6级":
                                    case "新阳灵石6级":
                                    case "韧紫灵石6级":
                                    case "守阳灵石6级":
                                    case "纯紫灵石6级":
                                    case "狂热幻彩灵石6级":
                                        num8 += 6000;
                                        break;
                                    case "透蓝灵石1级":
                                    case "驭朱灵石1级":
                                    case "韧紫灵石1级":
                                    case "守阳灵石1级":
                                    case "赤褐灵石1级":
                                    case "纯紫灵石1级":
                                    case "狂热幻彩灵石1级":
                                    case "精绿灵石1级":
                                    case "新阳灵石1级":
                                    case "盈绿灵石1级":
                                    case "蔚蓝灵石1级":
                                    case "橙黄灵石1级":
                                    case "深灰灵石1级":
                                    case "命朱灵石1级":
                                    case "进击幻彩灵石1级":
                                    case "抵御幻彩灵石1级":
                                        num8 += 1000;
                                        break;
                                    case "蔚蓝灵石10级":
                                    case "狂热幻彩灵石10级":
                                    case "精绿灵石10级":
                                    case "透蓝灵石10级":
                                    case "橙黄灵石10级":
                                    case "抵御幻彩灵石10级":
                                    case "进击幻彩灵石10级":
                                    case "命朱灵石10级":
                                    case "守阳灵石10级":
                                    case "赤褐灵石10级":
                                    case "盈绿灵石10级":
                                    case "深灰灵石10级":
                                    case "韧紫灵石10级":
                                    case "纯紫灵石10级":
                                    case "新阳灵石10级":
                                    case "驭朱灵石10级":
                                        num8 += 10000;
                                        break;
                                    case "驭朱灵石3级":
                                    case "韧紫灵石3级":
                                    case "精绿灵石3级":
                                    case "新阳灵石3级":
                                    case "守阳灵石3级":
                                    case "盈绿灵石3级":
                                    case "蔚蓝灵石3级":
                                    case "命朱灵石3级":
                                    case "橙黄灵石3级":
                                    case "进击幻彩灵石3级":
                                    case "抵御幻彩灵石3级":
                                    case "透蓝灵石3级":
                                    case "赤褐灵石3级":
                                    case "深灰灵石3级":
                                    case "狂热幻彩灵石3级":
                                    case "纯紫灵石3级":
                                        num8 += 3000;
                                        break;
                                }
                            }
                        }
                        int num9 = num3 + num4 + num5 + num6 + num7 + num8;
                        decimal num10 = (decimal)v3 / (decimal)num2 * 0.9m * (decimal)num9;
                        decimal num11 = (decimal)num9 * 0.1m;
                        return (int)(num10 + num11);
                    }
                case PersistentItemType.消耗:
                    {
                        int v2 = Dura.V;
                        int 物品持久 = Info.MaxDura;
                        int num = Info.SalePrice;
                        return (int)((decimal)v2 / (decimal)物品持久 * (decimal)num);
                    }
                case PersistentItemType.Stack:
                    {
                        int v = Dura.V;
                        return Info.SalePrice * v;
                    }
                case PersistentItemType.回复:
                    return 1;
                case PersistentItemType.容器:
                    return Info.SalePrice;
                case PersistentItemType.纯度:
                    return Info.SalePrice;
            }
        }
    }

    public int StackSize => Info.MaxDura;

    public int 默认持久
    {
        get
        {
            if (PersistType != PersistentItemType.Equipment)
                return Info.MaxDura;
            return Info.MaxDura * 1000;
        }
    }

    public byte Position
    {
        get { return Location.V; }
        set { Location.V = value; }
    }

    public bool IsBound => Info.IsBound;
    public bool IsResourceItem => Info.IsResourceItem;
    public bool CanSell => Info.CanSell;
    public bool CanStack => Info.PersistType == PersistentItemType.Stack;
    public bool 背包锁定 => Info.背包锁定;
    public bool CanDrop => Info.CanDrop;
    public int 称号编号值 => Info.称号编号值;
    public ushort AdditionalSkill => Info.AdditionalSkill;
    public int CurrencyModel => Info.CurrencyModel;
    public byte MountID => Info.MountID;
    public byte ChestID => Info.ChestID;
    public byte TeleportationAreaID => Info.TeleportationAreaID;
    public int 给予物品 => Info.给予物品;
    public int 给予物品数量 => Info.给予物品数量;
    public int 货币面额 => Info.货币面额;
    public int HealthAmount => Info.HealthAmount;
    public int 书页分解 => Info.书页分解;
    public int ManaAmount => Info.ManaAmount;
    public int Experience => Info.Experience;
    public int DrugModel => Info.DrugModel;
    public int DrugIntervalTime => Info.DrugIntervalTime;
    public int MaxUseCount => Info.使用次数;
    public byte GroupID => Info.GroupID;
    public int GroupCooling => Info.GroupCooling;
    public int BuffDrugID => Info.BuffDrugID;
    public ushort BuffID => Info.BuffID;
    public int Cooldown => Info.Cooldown;

    public ItemInfo()
    {
    }

    public ItemInfo(GameItem item, CharacterInfo owner, byte grid, byte position, int durability = int.MaxValue)
    {
        Item.V = item;
        Owner.V = owner;
        Grid.V = grid;
        Location.V = position;
        CreatedDate.V = SEngine.CurrentTime;
        MaxDura.V = Info.MaxDura;
        Dura.V = Math.Min(durability, MaxDura.V);
        Session.ItemInfoTable.Add(this, true);
    }

    public override string ToString() => Name;

    public virtual byte[] ToArray() => ToArray(Dura.V);

    public virtual byte[] ToArray(int quantity)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(Version);
        writer.Write(Owner.V?.Index.V ?? 0);
        writer.Write(Compute.TimeSeconds(CreatedDate.V));
        writer.Write(Item.V.ID);
        writer.Write(Grid.V);
        writer.Write(Location.V);
        writer.Write(quantity);
        writer.Write(MaxDura.V);
        writer.Write((byte)(IsBound ? 10u : 0u));
        writer.Write((ushort)0);
        writer.Write(0);
        writer.Write(0); // Unknown
        return ms.ToArray();
    }
}
