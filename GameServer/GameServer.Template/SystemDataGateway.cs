using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer.Template;

public static class SystemDataGateway
{
    private static void LoadDataType(Type type)
    {
        var method = type.GetMethod("LoadData", BindingFlags.Static | BindingFlags.Public);

        if (method != null)
            method.Invoke(null, null);
        else
        {
            SMain.AddSystemLog(type.Name + " Failed to find 'LoadData' method, Failed to load");
            return;
        }

        var field = type.GetField("DataSheet", BindingFlags.Static | BindingFlags.Public);
        if (field == null)
        {
            SMain.AddSystemLog(type.Name + " Failed to find 'DataSheet' property, Failed to load");
            return;
        }

        var obj = field.GetValue(null);
        if (obj == null)
        {
            SMain.AddSystemLog(type.Name + " Failed to load content, Check data directory");
            return;
        }

        var property = obj.GetType().GetProperty("Count", BindingFlags.Instance | BindingFlags.Public);
        if (property == null)
        {
            SMain.AddSystemLog(type.Name + " Failed to find 'Count' property, Failed to load");
            return;
        }

        var count = (int)property.GetValue(obj);
        SMain.AddSystemLog($"{type.Name} Template loaded, Total: {count}");
    }

    private static void ReloadDataType(Type type)
    {
        Task.Run(delegate
        {
            LoadDataType(type);
        }).Wait();
    }

    public static void LoadMonsters() => ReloadDataType(typeof(MonsterInfo));

    public static void LoadGuards() => ReloadDataType(typeof(GuardInfo));

    public static void 对话数据() => ReloadDataType(typeof(NpcDialogs));

    public static void 游戏地图() => ReloadDataType(typeof(GameMap));
    
    public static void LoadTerrains() => ReloadDataType(typeof(Terrain));
    
    public static void 地图区域() => ReloadDataType(typeof(MapArea));
    
    public static void 传送法阵() => ReloadDataType(typeof(TeleportGate));
    
    public static void 怪物刷新() => ReloadDataType(typeof(MonsterSpawn));
    
    public static void 守卫刷新() => ReloadDataType(typeof(MapGuard));
    
    public static void 游戏物品() => ReloadDataType(typeof(GameItem));
 
    public static void 随机属性() => ReloadDataType(typeof(RandomStats));

    public static void 装备属性() => ReloadDataType(typeof(EquipmentStats));

    public static void 游戏商店() => ReloadDataType(typeof(GameStore));

    public static void 珍宝商品() => ReloadDataType(typeof(RareTreasureItem));
    
    public static void 游戏称号() => ReloadDataType(typeof(GameTitle));

    public static void 铭文技能() => ReloadDataType(typeof(InscriptionSkill));

    public static void 游戏技能() => ReloadDataType(typeof(GameSkill));

    public static void 技能陷阱() => ReloadDataType(typeof(SkillTrap));

    public static void 游戏Buff() => ReloadDataType(typeof(GameBuff));
    
    public static void 游戏坐骑() => ReloadDataType(typeof(GameMount));

    public static void 套装数据() => ReloadDataType(typeof(ItemSetInfo));

    public static void 坐骑御兽() => ReloadDataType(typeof(MountBeast));

    public static void 合成数据() => ReloadDataType(typeof(ItemCrafting));

    public static void VIP数据() => ReloadDataType(typeof(VIPSystem));

    public static void 宝箱数据() => ReloadDataType(typeof(TreasureChestInfo));
   
    public static void ReloadData()
    {
        List<Type> templates = new List<Type>
        {
            typeof(MonsterInfo),
            typeof(GuardInfo),
            typeof(NpcDialogs),
            typeof(GameMap),
            typeof(Terrain),
            typeof(MapArea),
            typeof(TeleportGate),
            typeof(MonsterSpawn),
            typeof(MapGuard),
            typeof(GameItem),
            typeof(RandomStats),
            typeof(EquipmentStats),
            typeof(GameStore),
            typeof(RareTreasureItem),
            typeof(GameTitle),
            typeof(InscriptionSkill),
            typeof(GameSkill),
            typeof(SkillTrap),
            typeof(GameBuff),
            typeof(GameMount),
            typeof(ItemSetInfo),
            typeof(MountBeast),
            typeof(ItemCrafting),
            typeof(VIPSystem),
            typeof(TreasureChestInfo)
        };
        Task.Run(delegate
        {
            foreach (var type in templates)
                LoadDataType(type);
        }).Wait();
        SMain.AddSystemLog("Text data loaded...");
    }

    public static void LoadData()
    {
        List<Type> templates = new List<Type>
        {
            typeof(MonsterInfo),
            typeof(GuardInfo),
            typeof(NpcDialogs),
            typeof(GameMap),
            typeof(Terrain),
            typeof(MapArea),
            typeof(TeleportGate),
            typeof(MonsterSpawn),
            typeof(MapGuard),
            typeof(GameItem),
            typeof(RandomStats),
            typeof(EquipmentStats),
            typeof(GameStore),
            typeof(RareTreasureItem),
            typeof(GameTitle),
            typeof(InscriptionSkill),
            typeof(GameSkill),
            typeof(SkillTrap),
            typeof(GameBuff),
            typeof(GameMount),
            typeof(ItemSetInfo),
            typeof(MountBeast),
            typeof(ItemCrafting),
            typeof(VIPSystem),
            typeof(TreasureChestInfo)
        };

        Task.Run(delegate
        {
            foreach (var type in templates)
                LoadDataType(type);
        }).Wait();
    }
}
