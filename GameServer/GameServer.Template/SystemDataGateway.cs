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
        Task.Run(() => { LoadDataType(type); }).Wait();
    }

    public static void LoadMonsterInfos() => ReloadDataType(typeof(MonsterInfo));

    public static void LoadGuardInfos() => ReloadDataType(typeof(GuardInfo));

    public static void LoadNpcDialogs() => ReloadDataType(typeof(NpcDialog));

    public static void LoadGameMaps() => ReloadDataType(typeof(GameMap));
    
    public static void LoadTerrains() => ReloadDataType(typeof(Terrain));
    
    public static void LoadMapAreas() => ReloadDataType(typeof(MapArea));
    
    public static void LoadTeleportGates() => ReloadDataType(typeof(TeleportGate));
    
    public static void LoadMonsterSpawns() => ReloadDataType(typeof(MonsterSpawn));

    public static void LoadMonsterBossTimedZen() => ReloadDataType(typeof(MonsterBossTimedZen));

    public static void LoadMapGuards() => ReloadDataType(typeof(MapGuard));
    
    public static void LoadGameItems() => ReloadDataType(typeof(GameItem));
 
    public static void LoadRandomStats() => ReloadDataType(typeof(RandomStats));

    public static void LoadEquipmentStats() => ReloadDataType(typeof(EquipmentStats));

    public static void LoadGameStores() => ReloadDataType(typeof(GameStore));

    public static void LoadRareTreasureItems() => ReloadDataType(typeof(RareTreasureItem));
    
    public static void LoadGameTitles() => ReloadDataType(typeof(GameTitle));

    public static void LoadInscriptionSkills() => ReloadDataType(typeof(InscriptionSkill));

    public static void LoadGameSkills() => ReloadDataType(typeof(GameSkill));

    public static void LoadSkillTraps() => ReloadDataType(typeof(SkillTrap));

    public static void LoadGameBuffs() => ReloadDataType(typeof(GameBuff));
    
    public static void LoadGameMounts() => ReloadDataType(typeof(GameMount));

    public static void LoadItemSetInfos() => ReloadDataType(typeof(ItemSetInfo));

    public static void LoadMountBeasts() => ReloadDataType(typeof(MountStats));

    public static void LoadItemCraftings() => ReloadDataType(typeof(ItemCrafting));

    public static void LoadVIPSystem() => ReloadDataType(typeof(VIPSystem));

    public static void LoadTreasureChestInfos() => ReloadDataType(typeof(TreasureChestInfo));

    public static void LoadAdministrators() => ReloadDataType(typeof(Administrator));

    public static void ReloadData()
    {
        List<Type> templates = new List<Type>
        {
            typeof(MonsterInfo),
            typeof(GuardInfo),
            typeof(NpcDialog),
            typeof(GameMap),
            typeof(Terrain),
            typeof(MapArea),
            typeof(TeleportGate),
            typeof(MonsterSpawn),
            typeof(MonsterBossTimedZen),
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
            typeof(MountStats),
            typeof(ItemCrafting),
            typeof(VIPSystem),
            typeof(TreasureChestInfo),
            typeof(Administrator)
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
            typeof(NpcDialog),
            typeof(GameMap),
            typeof(Terrain),
            typeof(MapArea),
            typeof(TeleportGate),
            typeof(MonsterSpawn),
            typeof(MonsterBossTimedZen),
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
            typeof(MountStats),
            typeof(ItemCrafting),
            typeof(VIPSystem),
            typeof(TreasureChestInfo),
            typeof(Administrator)
        };

        Task.Run(delegate
        {
            foreach (var type in templates)
                LoadDataType(type);
        }).Wait();
    }
}
