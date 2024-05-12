using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                case PersistentItemType.None:
                    return 1;
                case PersistentItemType.Equipment:
                    {
                        if (this is not EquipmentInfo equipInfo)
                            return 0;

                        int dura = equipInfo.Dura.V;
                        int luck = Math.Max((sbyte)0, equipInfo.Luck.V);
                        int pwr = equipInfo.DCPower.V * 100 + equipInfo.MCPower.V * 100 + equipInfo.SCPower.V * 100 + equipInfo.NCPower.V * 100 + equipInfo.BCPower.V * 100;
                        
                        int icount = equipInfo.InscriptionSkills.Values.Count(x => x != null);

                        int rbonus = 0;
                        foreach (RandomStats stat in equipInfo.RandomStats)
                            rbonus += stat.CombatBonus * 100;
                        
                        int num8 = 0;
                        foreach (var item in equipInfo.镶嵌灵石.Values)
                        {
                            switch (item.Name)
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
                        int price = Info.SalePrice + luck + pwr + icount + rbonus + num8;
                        decimal num10 = (decimal)dura / (decimal)(Info.MaxDura * 1000) * 0.9m * (decimal)price;
                        decimal num11 = (decimal)price * 0.1m;
                        return (int)(num10 + num11);
                    }
                case PersistentItemType.Consumeable:
                    {
                        return (int)((decimal)Dura.V / (decimal)Info.MaxDura * (decimal)Info.SalePrice);
                    }
                case PersistentItemType.Stack:
                    {
                        return Info.SalePrice * Dura.V;
                    }
                case PersistentItemType.回复:
                    return 1;
                case PersistentItemType.Container:
                case PersistentItemType.Purity:
                    return Info.SalePrice;
                default:
                    return 0;
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
    public int TitleID => Info.称号编号值;
    public ushort AdditionalSkill => Info.AdditionalSkill;
    public int CurrencyModel => Info.CurrencyModel;
    public byte MountID => Info.MountID;
    public byte ChestID => Info.ChestID;
    public byte TeleportationAreaID => Info.TeleportationAreaID;
    public int GrantedItemID => Info.给予物品;
    public int GrantedItemCount => Info.给予物品数量;
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
