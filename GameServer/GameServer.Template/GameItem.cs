using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public class GameItem
{
    public static Dictionary<int, GameItem> DataSheet = new Dictionary<int, GameItem>();
    public static Dictionary<string, GameItem> DataSheetByName = new Dictionary<string, GameItem>();

    public string Name;
    public int ID;
    public int MaxDura;
    public int Weight;
    public int Level;
    public int NeedLevel;
    public int Cooldown;
    public byte GroupID;
    public int GroupCooling;
    public int SalePrice;
    public ushort AdditionalSkill;
    public bool IsBound;
    public bool CanBeBrokenDown;
    public bool CanDrop;
    public bool CanSell;
    public bool ValuableObjects;
    public bool IsResourceItem;
    public int HealthAmount;
    public int ManaAmount;
    public int 使用次数;
    public int DrugIntervalTime;
    public int DrugModel;
    public int CurrencyModel;
    public int 货币面额;
    public int 称号编号值;
    public bool 背包锁定;
    public int 书页分解;
    public int 给予物品;
    public int 给予物品数量;
    public byte MountID;
    public byte TeleportationAreaID;
    public int BuffDrugID;
    public byte ChestID;
    public ushort BuffID;
    public int Experience;

    public ItemType Type;
    public GameObjectRace NeedRace;
    public GameObjectGender NeedGender;
    public PersistentItemType PersistType;
    public SaleType StoreType;
    public GameItemSet 装备套装提示;
    public GameItemSet 属性装备套装;

    public static GameItem GetItem(int id)
    {
        if (DataSheet.TryGetValue(id, out var value))
            return value;
        return null;
    }

    public static GameItem GetItem(string name)
    {
        if (DataSheetByName.TryGetValue(name, out var value))
            return value;
        return null;
    }

    public static void LoadData()
    {
        DataSheet.Clear();
        DataSheetByName.Clear();

        var path = Settings.Default.GameDataPath + "\\System\\Items\\Common\\";
        if (Directory.Exists(path))
        {
            var array = Serializer.Deserialize<GameItem>(path);
            foreach (var obj in array)
            {
                DataSheet.Add(obj.ID, obj);
                DataSheetByName.Add(obj.Name, obj);
            }
        }

        path = Settings.Default.GameDataPath + "\\System\\Items\\Equipment\\";
        if (Directory.Exists(path))
        {
            var array = Serializer.Deserialize<EquipmentItem>(path);
            foreach (var obj in array)
            {
                DataSheet.Add(obj.ID, obj);
                DataSheetByName.Add(obj.Name, obj);
            }
        }
    }
}
