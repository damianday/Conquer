
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Drawing;

var jsonOptions = new JsonSerializerSettings
{
    DefaultValueHandling = DefaultValueHandling.Ignore,
    NullValueHandling = NullValueHandling.Ignore,
    TypeNameHandling = TypeNameHandling.None,
    Formatting = Formatting.Indented
};

var chineseDbSystemPath = @"D:\Conquera\DatabaseChinese\System";
var mir3dDbSystemPath = @"D:\Conquera\Database\System";

var UseReadInt = false;
var UseWriteInt = false;

void DumpBuffs()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "技能数据", "Buff数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Skills", "Buffs");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "Buff名字":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "Buff编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "分组编号":
                    chineseConverted["GroupID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "作用类型":
                    chineseConverted["ActionType"] = ConvertBuffActionType(chineseProp.Value.Value<string>());
                    break;
                case "叠加类型":
                    chineseConverted["StackingType"] = ConvertBuffStackType(chineseProp.Value.Value<string>());
                    break;
                case "Buff效果":
                    chineseConverted["Effect"] = ConvertBuffEffectType(chineseProp.Value.Value<string>());
                    break;
                case "同步至客户端":
                    chineseConverted["SyncClient"] = chineseProp.Value.Value<bool>();
                    break;
                case "到期主动消失":
                    chineseConverted["RemoveOnExpire"] = chineseProp.Value.Value<bool>();
                    break;
                case "切换地图消失":
                    chineseConverted["OnChangeMapRemove"] = chineseProp.Value.Value<bool>();
                    break;
                case "切换武器消失":
                    chineseConverted["OnChangeWeaponRemove"] = chineseProp.Value.Value<bool>();
                    break;
                case "角色死亡消失":
                    chineseConverted["OnPlayerDiesRemove"] = chineseProp.Value.Value<bool>();
                    break;
                case "角色下线消失":
                    chineseConverted["OnPlayerDisconnectRemove"] = chineseProp.Value.Value<bool>();
                    break;
                case "绑定技能等级":
                    chineseConverted["BindingSkillLevel"] = chineseProp.Value.Value<ushort>();
                    break;
                case "释放技能消失":
                    chineseConverted["OnReleaseSkillRemove"] = chineseProp.Value.Value<bool>();
                    break;
                case "移除添加冷却":
                    chineseConverted["RemoveAddCooling"] = chineseProp.Value.Value<bool>();
                    break;
                case "技能冷却时间":
                    chineseConverted["SkillCooldown"] = chineseProp.Value.Value<ushort>();
                    break;
                case "Buff初始层数":
                    chineseConverted["InitialBuffStacks"] = chineseProp.Value.Value<byte>();
                    break;
                case "Buff最大层数":
                    chineseConverted["MaxBuffCount"] = chineseProp.Value.Value<byte>();
                    break;
                case "Buff允许合成":
                    chineseConverted["AllowsSynthesis"] = chineseProp.Value.Value<bool>();
                    break;
                case "Buff合成层数":
                    chineseConverted["BuffCraftingStacks"] = chineseProp.Value.Value<byte>();
                    break;
                case "Buff合成编号":
                    chineseConverted["BuffCraftingID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "Buff处理间隔":
                    chineseConverted["ProcessInterval"] = chineseProp.Value.Value<int>();
                    break;
                case "Buff处理延迟":
                    chineseConverted["ProcessDelay"] = chineseProp.Value.Value<int>();
                    break;
                case "Buff持续时间":
                    chineseConverted["Duration"] = chineseProp.Value.Value<int>();
                    break;
                case "持续时间延长":
                    chineseConverted["ExtendedDuration"] = chineseProp.Value.Value<bool>();
                    break;
                case "后接Buff编号":
                    chineseConverted["FollowedByID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "连带Buff编号":
                    chineseConverted["AssociatedID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "依存Buff列表":
                    chineseConverted["RequireBuff"] = chineseProp.Value.Values<ushort>().ToList();
                    break;
                case "技能等级延时":
                    chineseConverted["SkillLevelDelay"] = chineseProp.Value.Value<bool>();
                    break;
                case "每级延长时间":
                    chineseConverted["ExtendedTimePerLevel"] = chineseProp.Value.Value<int>();
                    break;
                case "角色属性延时":
                    chineseConverted["PlayerStatDelay"] = chineseProp.Value.Value<bool>();
                    break;
                case "绑定角色属性":
                    chineseConverted["BoundPlayerStat"] = ConvertStat(chineseProp.Value.Value<string>());
                    break;
                case "属性延时系数":
                    chineseConverted["StatDelayFactor"] = chineseProp.Value.Value<float>();
                    break;
                case "特定铭文延时":
                    chineseConverted["HasSpecificInscriptionDelay"] = chineseProp.Value.Value<bool>();
                    break;
                case "特定铭文技能":
                    chineseConverted["SpecificInscriptionSkills"] = chineseProp.Value.Value<int>();
                    break;
                case "铭文延长时间":
                    chineseConverted["InscriptionExtendedTime"] = chineseProp.Value.Value<int>();
                    break;
                case "角色所处状态":
                    chineseConverted["PlayerState"] = ConvertObjectState(chineseProp.Value.Value<string>());
                    break;
                case "属性增减":
                    chineseConverted["StatsIncOrDec"] = chineseProp.Value.Values<JObject>().Select(x => ConvertInscriptionStat(x)).ToList();
                    break;
                case "Buff伤害类型":
                    chineseConverted["DamageType"] = ConvertSkillDamageType(chineseProp.Value.Value<string>());
                    break;
                case "Buff伤害基数":
                    chineseConverted["DamageBase"] = chineseProp.Value.Values<int>().ToList();
                    break;
                case "Buff伤害系数":
                    chineseConverted["DamageFactor"] = chineseProp.Value.Values<float>().ToList();
                    break;
                case "强化铭文编号":
                    chineseConverted["StrengthenInscriptionID"] = chineseProp.Value.Value<int>();
                    break;
                case "铭文强化基数":
                    chineseConverted["StrengthenInscriptionBase"] = chineseProp.Value.Value<int>();
                    break;
                case "铭文强化系数":
                    chineseConverted["InscriptionEnhancementFactor"] = chineseProp.Value.Value<float>();
                    break;
                case "效果生效移除":
                    chineseConverted["EffectRemoved"] = chineseProp.Value.Value<bool>();
                    break;
                case "生效后接编号":
                    chineseConverted["EffectiveFollowedByID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "后接技能来源":
                    chineseConverted["FollowedBySkillOwner"] = chineseProp.Value.Value<bool>();
                    break;
                case "效果判定方式":
                    chineseConverted["HowJudgeEffect"] = ConvertBuffDetherminationMethod(chineseProp.Value.Value<string>());
                    break;
                case "限定伤害上限":
                    chineseConverted["LimitedDamage"] = chineseProp.Value.Value<bool>();
                    break;
                case "限定伤害数值":
                    chineseConverted["LimitedDamageValue"] = chineseProp.Value.Value<int>();
                    break;
                case "效果判定类型":
                    chineseConverted["EffectJudgeType"] = ConvertBuffDetherminationType(chineseProp.Value.Value<string>());
                    break;
                case "特定技能编号":
                    chineseConverted["SpecificSkillID"] = chineseProp.Value.Values<ushort>().ToList();
                    break;
                case "伤害增减基数":
                    chineseConverted["DamageIncOrDecBase"] = chineseProp.Value.Values<int>().ToList();
                    break;
                case "伤害增减系数":
                    chineseConverted["DamageIncOrDecFactor"] = chineseProp.Value.Values<float>().ToList();
                    break;
                case "触发陷阱技能":
                    chineseConverted["TriggerTrapSkills"] = chineseProp.Value.Value<string>();
                    break;
                case "触发陷阱数量":
                    chineseConverted["NumberTrapsTriggered"] = ConvertObjectSize(chineseProp.Value.Value<string>());
                    break;
                case "体力回复基数":
                    chineseConverted["PhysicalRecoveryBase"] = chineseProp.Value.Value<string>(); //chineseProp.Value.Values<byte>().ToList();
                    break;
                case "诱惑时长增加":
                    chineseConverted["TemptationDurationIncreased"] = chineseProp.Value.Value<int>();
                    break;
                case "诱惑概率增加":
                    chineseConverted["IncreasedTemptationChance"] = chineseProp.Value.Value<float>();
                    break;
                case "诱惑等级增加":
                    chineseConverted["TemptationLevelIncreased"] = chineseProp.Value.Value<byte>();
                    break;
                
                /*case "每层触发Buff":
                    chineseConverted["每层触发Buff"] = chineseProp.Value.Values<int>().ToList();
                    break;
                case "特定BUFF编号":
                    chineseConverted["SpecificBuffID"] = chineseProp.Value.Values<ushort>().ToList();
                    break;
                case "体力回复系数":
                    chineseConverted["HealthRegenerationFactor"] = chineseProp.Value.Values<float>().ToList();
                    break;
                case "魔力回复基数":
                    chineseConverted["ManaRecoveryBase"] = chineseProp.Value.Value<string>();
                    break;
                case "魔力回复系数":
                    chineseConverted["ManaRegenerationFactor"] = chineseProp.Value.Values<float>().ToList();
                    break;
                case "Buff备注":
                    chineseConverted["BuffNote"] = chineseProp.Value.Value<string>();
                    break;
                case "限定目标类型":
                    chineseConverted["LimitedTargetType"] = ConvertObjectType(chineseProp.Value.Value<string>());
                    break;
                case "不算BUFF层数":
                    chineseConverted["DoesNotCountBuffStacks"] = chineseProp.Value.Value<bool>();
                    break;
                case "攻击触发技能":
                    chineseConverted["AttackTriggerSkill"] = chineseProp.Value.Value<string>().Split('-', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                    break;
                case "效果判定取反":
                    chineseConverted["ReverseJudgeEffect"] = chineseProp.Value.Value<bool>();
                    break;
                case "攻击动作消失":
                    chineseConverted["AttackAnimationDisappears"] = chineseProp.Value.Value<bool>();
                    break;
                case "触发技能伤害":
                    chineseConverted["TriggerSpellDamage"] = chineseProp.Value.Value<ushort>();
                    break;
                case "伤害不计神圣":
                    chineseConverted["SacredDamage"] = chineseProp.Value.Value<bool>();
                    break;*/
                default:
                    throw new ApplicationException();
            }
        }

        var chineseId = chineseModel.Value<int>("Buff编号");
        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseId);
        var mir3dOutputPath = Path.Combine(mir3DbFolder, $"{chineseConverted["ID"]}-{chineseConverted["Name"]}.txt");

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpSkillsData()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "技能数据", "技能数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Skills", "Skills");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "技能名字":
                    chineseConverted["SkillName"] = chineseProp.Value.Value<string>();
                    break;
                case "技能职业":
                    chineseConverted["Race"] = ConvertObjectRace(chineseProp.Value.Value<string>());
                    break;
                case "技能类型":
                    chineseConverted["SkillType"] = ConvertGameSkillType(chineseProp.Value.Value<string>());
                    break;
                case "自身技能编号":
                    chineseConverted["OwnSkillID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "自身铭文编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<byte>();
                    break;
                case "技能分组编号":
                    chineseConverted["GroupID"] = chineseProp.Value.Value<byte>();
                    break;
                case "绑定等级编号":
                    chineseConverted["BindingLevelID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "需要正向走位":
                    chineseConverted["NeedMoveForward"] = chineseProp.Value.Value<bool>();
                    break;
                case "技能最远距离":
                    chineseConverted["MaxDistance"] = chineseProp.Value.Value<byte>();
                    break;
                case "计算幸运概率":
                    chineseConverted["CalculateLuckyProbability"] = chineseProp.Value.Value<bool>();
                    break;
                case "计算触发概率":
                    chineseConverted["CalculateTriggerProbability"] = chineseProp.Value.Value<float>();
                    break;
                case "属性提升概率":
                    chineseConverted["StatBoostProbability"] = ConvertStat(chineseProp.Value.Value<string>());
                    break;
                case "属性提升系数":
                    chineseConverted["StatBoostFactor"] = chineseProp.Value.Value<float>();
                    break;
                case "检查忙绿状态":
                    chineseConverted["CheckBusyGreen"] = chineseProp.Value.Value<bool>();
                    break;
                case "检查硬直状态":
                    chineseConverted["CheckStunStatus"] = chineseProp.Value.Value<bool>();
                    break;
                case "检查职业武器":
                    chineseConverted["CheckOccupationalWeapons"] = chineseProp.Value.Value<bool>();
                    break;
                case "检查被动标记":
                    chineseConverted["CheckPassiveTags"] = chineseProp.Value.Value<bool>();
                    break;
                case "检查技能标记":
                    chineseConverted["CheckSkillMarks"] = chineseProp.Value.Value<bool>();
                    break;
                case "检查技能计数":
                    chineseConverted["CheckSkillCount"] = chineseProp.Value.Value<bool>();
                    break;
                case "技能标记编号":
                    chineseConverted["SkillTagID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "需要消耗魔法":
                    chineseConverted["NeedConsumeMagic"] = chineseProp.Value.Values<int>().ToList();
                    break;
                case "需要消耗物品":
                    chineseConverted["NeedConsumeItems"] = chineseProp.Value.Values<int>().ToArray();
                    break;
                case "消耗物品数量":
                    chineseConverted["NeedConsumeItemsQuantity"] = chineseProp.Value.Value<int>();
                    break;
                case "战具扣除点数":
                    chineseConverted["GearDeductionPoints"] = chineseProp.Value.Value<int>();
                    break;
                case "验证已学技能":
                    chineseConverted["ValidateLearnedSkills"] = chineseProp.Value.Value<ushort>();
                    break;
                case "验证技能铭文":
                    chineseConverted["VerficationSkillInscription"] = chineseProp.Value.Value<byte>();
                    break;
                case "验证角色Buff":
                    chineseConverted["VerifyPlayerBuff"] = chineseProp.Value.Value<ushort>();
                    break;
                case "角色Buff层数":
                    chineseConverted["PlayerBuffStackCount"] = chineseProp.Value.Value<int>();
                    break;
                case "验证目标类型":
                    chineseConverted["VerifyTargetType"] = ConvertSpecifyTargetType(chineseProp.Value.Value<string>());
                    break;
                case "验证目标Buff":
                    chineseConverted["VerifyTargetBuff"] = chineseProp.Value.Value<ushort>();
                    break;
                case "目标Buff层数":
                    chineseConverted["TargetBuffStackCount"] = chineseProp.Value.Value<int>();
                    break;
                case "目标最近距离":
                    chineseConverted["TargetClosestDistance"] = chineseProp.Value.Value<int>();
                    break;
                case "目标最远距离":
                    chineseConverted["TargetFurthestDistance"] = chineseProp.Value.Value<int>();
                    break;
                case "自动装配":
                    chineseConverted["AutomaticAssembly"] = chineseProp.Value.Value<bool>();
                    break;
                case "友方技能编号":
                    chineseConverted[chineseProp.Key] = chineseProp.Value.Value<ushort>();
                    break;
                case "增加技能经验":
                    chineseConverted[chineseProp.Key] = chineseProp.Value.Value<bool>();
                    break;
                case "节点列表":
                    chineseConverted["Nodes"] = chineseProp.Value.Values<JProperty>().ToDictionary(x => x.Name, y => ConvertSkillNode((JObject)y.Value));
                    break;

                default:
                    throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<string>("SkillName") == chineseModel.Value<string>("技能名字"));

        var name = chineseConverted["SkillName"];
        var ownSkillId = chineseConverted.ContainsKey("OwnSkillId") ? (int)chineseConverted["OwnSkillId"] : 0;
        var id = chineseConverted.ContainsKey("Id") ? (int)chineseConverted["Id"] : 0;
        var fileName = $"{ownSkillId}-{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
        else
        {
            //if (CompareRecursiveValue(chineseConverted, mir3dModel, new string[] { "SkillName" }))
            //{
            //    var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            //    File.WriteAllText(mir3dOutputPath, content);
            //}
        }
    }
}

void DumpSkillInscriptions()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "技能数据", "铭文数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Skills", "Inscriptions");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "技能名字":
                    chineseConverted["SkillName"] = chineseProp.Value.Value<string>();
                    break;
                case "技能职业":
                    chineseConverted["Race"] = ConvertObjectRace(chineseProp.Value.Value<string>());
                    break;
                case "技能编号":
                    chineseConverted["SkillID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "铭文编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<byte>();
                    break;
                case "技能计数":
                    chineseConverted["SkillCount"] = chineseProp.Value.Value<byte>();
                    break;
                case "计数周期":
                    chineseConverted["PeriodCount"] = chineseProp.Value.Value<ushort>();
                    break;
                case "被动技能":
                    chineseConverted["PassiveSkill"] = chineseProp.Value.Value<bool>();
                    break;
                case "铭文品质":
                    chineseConverted["Quality"] = chineseProp.Value.Value<byte>();
                    break;
                case "洗练概率":
                    chineseConverted["Probability"] = chineseProp.Value.Value<int>();
                    break;
                case "广播通知":
                    chineseConverted["BroadcastNotification"] = chineseProp.Value.Value<bool>();
                    break;
                case "铭文描述":
                    chineseConverted["Description"] = chineseProp.Value.Value<string>();
                    break;
                case "需要角色等级":
                    chineseConverted["MinPlayerLevel"] = chineseProp.Value.Value<string>();
                    break;
                case "需要技能经验":
                    chineseConverted["MinSkillExp"] = chineseProp.Value.Values<int>().ToArray();
                    break;
                case "技能战力加成":
                    chineseConverted["SkillCombatBonus"] = chineseProp.Value.Values<int>().ToArray();
                    break;
                case "铭文属性加成":
                    chineseConverted["StatsBonus"] = chineseProp.Value.Values<JObject>().Select(x => ConvertInscriptionStat(x)).ToArray();
                    break;
                case "铭文附带Buff":
                    chineseConverted["ComesWithBuff"] = chineseProp.Value.Values<int>().ToArray();
                    break;
                case "被动技能列表":
                    chineseConverted["PassiveSkills"] = chineseProp.Value.Values<int>().ToArray();
                    break;
                case "主体技能列表":
                    chineseConverted["MainSkills"] = chineseProp.Value.Values<string>().ToArray();
                    break;
                case "开关技能列表":
                    chineseConverted["SwitchSkills"] = chineseProp.Value.Values<string>().ToArray();
                    break;
                case "角色所处状态":
                    chineseConverted[chineseProp.Key] = ConvertObjectState(chineseProp.Value.Value<string>());
                    break;
                case "角色死亡消失":
                    chineseConverted["RemoveOnDie"] = chineseProp.Value.Value<bool>();
                    break;

                default:
                    throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("SkillID") == chineseModel.Value<int>("技能编号"));

        var name = chineseConverted["SkillName"];
        var skillId = chineseConverted["SkillID"];
        var id = chineseConverted.ContainsKey("ID") ? (byte)chineseConverted["ID"] : 0;
        var fileName = $"{skillId}-{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpSkillTraps()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "技能数据", "陷阱数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Skills", "Traps");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile, System.Text.Encoding.UTF8);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));

    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "陷阱名字":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "陷阱编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "分组编号":
                case "陷阱分组编号":
                    chineseConverted["GroupID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "陷阱体型":
                    chineseConverted["Size"] = ConvertObjectSize(chineseProp.Value.Value<string>());
                    break;
                case "绑定等级":
                    chineseConverted["BindingLevel"] = chineseProp.Value.Value<ushort>();
                    break;
                case "陷阱允许叠加":
                    chineseConverted["AllowStacking"] = chineseProp.Value.Value<bool>();
                    break;
                case "陷阱持续时间":
                    chineseConverted["Duration"] = chineseProp.Value.Value<int>();
                    break;
                case "持续时间延长":
                    chineseConverted["ExtendedDuration"] = chineseProp.Value.Value<bool>();
                    break;
                case "技能等级延时":
                    chineseConverted["SkillLevelDelay"] = chineseProp.Value.Value<bool>();
                    break;
                case "每级延长时间":
                    chineseConverted["ExtendedTimePerLevel"] = chineseProp.Value.Value<int>();
                    break;
                case "角色属性延时":
                    chineseConverted["PlayerStatDelay"] = chineseProp.Value.Value<bool>();
                    break;
                case "绑定角色属性":
                    chineseConverted["BoundPlayerStat"] = ConvertStat(chineseProp.Value.Value<string>());
                    break;
                case "属性延时系数":
                    chineseConverted["StatDelayFactor"] = chineseProp.Value.Value<float>();
                    break;
                case "特定铭文延时":
                    chineseConverted["HasSpecificInscriptionDelay"] = chineseProp.Value.Value<bool>();
                    break;
                case "绑定铭文技能":
                    chineseConverted["BindInscriptionSkill"] = ConvertInscriptionSkill(chineseProp.Value.Value<JObject>());
                    break;
                case "特定铭文技能":
                    chineseConverted["SpecificInscriptionSkills"] = chineseProp.Value.Value<int>();
                    break;
                case "铭文延长时间":
                    chineseConverted["InscriptionExtendedTime"] = chineseProp.Value.Value<int>();
                    break;
                case "陷阱能否移动":
                    chineseConverted["CanMove"] = chineseProp.Value.Value<bool>();
                    break;
                case "陷阱移动速度":
                    chineseConverted["MoveSpeed"] = chineseProp.Value.Value<ushort>();
                    break;
                case "限制移动次数":
                    chineseConverted["LimitMoveSteps"] = chineseProp.Value.Value<byte>();
                    break;
                case "当前方向移动":
                    chineseConverted["MoveInCurrentDirection"] = chineseProp.Value.Value<bool>();
                    break;
                case "主动追击敌人":
                    chineseConverted["ActivelyPursueEnemy"] = chineseProp.Value.Value<bool>();
                    break;
                case "陷阱追击范围":
                    chineseConverted["PursuitRange"] = chineseProp.Value.Value<byte>();
                    break;
                case "被动触发技能":
                    chineseConverted["PassiveTriggerSkill"] = chineseProp.Value.Value<string>();
                    break;
                case "禁止重复触发":
                    chineseConverted["RetriggeringIsProhibited"] = chineseProp.Value.Value<bool>();
                    break;
                case "被动指定类型":
                    chineseConverted["PassiveTargetType"] = ConvertSpecifyTargetType(chineseProp.Value.Value<string>());
                    break;
                case "被动限定类型":
                    chineseConverted["PassiveObjectType"] = ConvertObjectType(chineseProp.Value.Value<string>());
                    break;
                case "被动限定关系":
                    chineseConverted["PassiveType"] = ConvertGameObjectRelationship(chineseProp.Value.Value<string>());
                    break;
                case "主动触发技能":
                    chineseConverted["ActivelyTriggerSkills"] = chineseProp.Value.Value<string>();
                    break;
                case "主动触发间隔":
                    chineseConverted["ActiveTriggerInterval"] = chineseProp.Value.Value<ushort>();
                    break;
                case "主动触发延迟":
                    chineseConverted["ActiveTriggerDelay"] = chineseProp.Value.Value<ushort>();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseModel.Value<int>("陷阱编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("ID") ? (ushort)chineseConverted["ID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

bool ParseGameItem(KeyValuePair<string, JToken?> chineseProp, Dictionary<string, object> chineseConverted)
{
    switch (chineseProp.Key)
    {
        case "物品名字":
            chineseConverted["Name"] = chineseProp.Value.Value<string>();
            break;
        case "物品编号":
            chineseConverted["ID"] = chineseProp.Value.Value<int>();
            break;
        case "物品持久":
            chineseConverted["MaxDura"] = chineseProp.Value.Value<int>();
            break;
        case "物品重量":
            chineseConverted["Weight"] = chineseProp.Value.Value<int>();
            break;
        case "物品等级":
            chineseConverted["Level"] = chineseProp.Value.Value<int>();
            break;
        case "需要等级":
            chineseConverted["NeedLevel"] = chineseProp.Value.Value<int>();
            break;
        case "冷却时间":
            chineseConverted["Cooldown"] = chineseProp.Value.Value<int>();
            break;
        case "物品分组":
            chineseConverted["GroupID"] = chineseProp.Value.Value<byte>();
            break;
        case "分组冷却":
            chineseConverted["GroupCooling"] = chineseProp.Value.Value<int>();
            break;
        case "出售价格":
            chineseConverted["SalePrice"] = chineseProp.Value.Value<int>();
            break;
        case "附加技能":
            chineseConverted["AdditionalSkill"] = chineseProp.Value.Value<ushort>();
            break;
        case "是否绑定":
            chineseConverted["IsBound"] = chineseProp.Value.Value<bool>();
            break;
        case "能否分解":
            chineseConverted["CanBeBrokenDown"] = chineseProp.Value.Value<bool>();
            break;
        case "能否掉落":
            chineseConverted["CanDrop"] = chineseProp.Value.Value<bool>();
            break;
        case "能否出售":
            chineseConverted["CanSell"] = chineseProp.Value.Value<bool>();
            break;
        case "贵重物品":
            chineseConverted["ValuableObjects"] = chineseProp.Value.Value<bool>();
            break;
        case "资源物品":
            chineseConverted["IsResourceItem"] = chineseProp.Value.Value<bool>();
            break;
        case "药品血量":
            chineseConverted["HealthAmount"] = chineseProp.Value.Value<int>();
            break;
        case "药品魔量":
            chineseConverted["ManaAmount"] = chineseProp.Value.Value<int>();
            break;
        case "使用次数":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
            break;
        case "药品间隔":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
            break;
        case "药品模式":
            chineseConverted["DrugModel"] = chineseProp.Value.Value<int>();
            break;
        case "货币模式":
            chineseConverted["CurrencyModel"] = chineseProp.Value.Value<int>();
            break;
        case "货币面额":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
            break;
        case "称号编号值":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
            break;
        case "背包锁定":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<bool>();
            break;
        case "书页分解":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
            break;
        case "给予物品":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
            break;
        case "给予物品数量":
            chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
            break;
        case "坐骑编号":
            chineseConverted["MountID"] = chineseProp.Value.Value<byte>();
            break;
        case "传送区域":
            chineseConverted["TeleportationAreaID"] = chineseProp.Value.Value<byte>();
            break;
        case "BUFF药品编号":
            chineseConverted["BuffDrugID"] = chineseProp.Value.Value<int>();
            break;
        case "宝箱编号":
            chineseConverted["ChestID"] = chineseProp.Value.Value<byte>();
            break;
        case "BUFF编号":
            chineseConverted["BuffID"] = chineseProp.Value.Value<ushort>();
            break;
        case "经验获得":
            chineseConverted["Experience"] = chineseProp.Value.Value<int>();
            break;
        case "物品分类":
            chineseConverted["Type"] = ConvertItemType(chineseProp.Value.Value<string>());
            break;
        case "需要职业":
            chineseConverted["NeedRace"] = ConvertObjectRace(chineseProp.Value.Value<string>());
            break;
        case "需要性别":
            chineseConverted["NeedGender"] = ConvertObjectGender(chineseProp.Value.Value<string>());
            break;
        case "持久类型":
            chineseConverted["PersistType"] = ConvertPersistentItemType(chineseProp.Value.Value<string>());
            break;
        case "商店类型":
            chineseConverted["StoreType"] = ConvertStoreType(chineseProp.Value.Value<string>());
            break;
        case "装备套装提示":
            chineseConverted[chineseProp.Key] = ConvertGameItemSet(chineseProp.Value.Value<string>());
            break;
        case "属性装备套装":
            chineseConverted[chineseProp.Key] = ConvertGameItemSet(chineseProp.Value.Value<string>());
            break;
        case "物品掉落列表":
            chineseConverted["Drops"] = chineseProp.Value.Values<JObject>().Select(x => ConvertMonsterDrop(x)).ToArray();
            break;

        default:
            return false;
    }

    return true;
}

void DumpCommonItems()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "物品数据", "普通物品");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "Common");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            if (!ParseGameItem(chineseProp, chineseConverted))
                throw new ApplicationException();
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseModel.Value<int>("物品编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("ID") ? (int)chineseConverted["ID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpEquipmentItems()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "物品数据", "装备物品");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "Equipment");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            if (!ParseGameItem(chineseProp, chineseConverted))
            {
                switch (chineseProp.Key)
                {
                    case "死亡销毁":
                        chineseConverted["DestroyOnDeath"] = chineseProp.Value.Value<bool>();
                        break;
                    case "禁止卸下":
                        chineseConverted["CanRemove"] = chineseProp.Value.Value<bool>();
                        break;
                    case "能否修理":
                        chineseConverted["CanRepair"] = chineseProp.Value.Value<bool>();
                        break;
                    case "修理花费":
                        chineseConverted["RepairCost"] = chineseProp.Value.Value<int>();
                        break;
                    case "特修花费":
                        chineseConverted["SpecialRepairCost"] = chineseProp.Value.Value<int>();
                        break;
                    case "需要攻击":
                        chineseConverted["NeedAttack"] = chineseProp.Value.Value<int>();
                        break;
                    case "需要魔法":
                        chineseConverted["NeedMagic"] = chineseProp.Value.Value<int>();
                        break;
                    case "需要道术":
                        chineseConverted["NeedTaoism"] = chineseProp.Value.Value<int>();
                        break;
                    case "需要刺术":
                        chineseConverted["NeedPiercing"] = chineseProp.Value.Value<int>();
                        break;
                    case "需要弓术":
                        chineseConverted["NeedArchery"] = chineseProp.Value.Value<int>();
                        break;
                    case "基础战力":
                        chineseConverted["BasePower"] = chineseProp.Value.Value<int>();
                        break;
                    case "最小攻击":
                        chineseConverted["MinDC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大攻击":
                        chineseConverted["MaxDC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最小魔法":
                        chineseConverted["MinMC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大魔法":
                        chineseConverted["MaxMC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最小道术":
                        chineseConverted["MinSC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大道术":
                        chineseConverted["MaxSC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最小刺术":
                        chineseConverted["MinNC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大刺术":
                        chineseConverted["MaxNC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最小弓术":
                        chineseConverted["MinBC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大弓术":
                        chineseConverted["MaxBC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最小防御":
                        chineseConverted["MinDef"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大防御":
                        chineseConverted["MaxDef"] = chineseProp.Value.Value<int>();
                        break;
                    case "最小魔防":
                        chineseConverted["MinMCDef"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大魔防":
                        chineseConverted["MaxMCDef"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大体力":
                        chineseConverted["MaxHP"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大魔力":
                        chineseConverted["MaxMP"] = chineseProp.Value.Value<int>();
                        break;
                    case "物理准确":
                        chineseConverted["PhysicalAccuracy"] = chineseProp.Value.Value<int>();
                        break;
                    case "物理敏捷":
                        chineseConverted["PhysicalAgility"] = chineseProp.Value.Value<int>();
                        break;
                    case "攻击速度":
                        chineseConverted["AttackSpeed"] = chineseProp.Value.Value<int>();
                        break;
                    case "幸运等级":
                        chineseConverted["Luck"] = chineseProp.Value.Value<int>();
                        break;
                    case "怪物伤害":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
                        break;
                    case "魔法闪避":
                        chineseConverted["MagicEvade"] = chineseProp.Value.Value<int>();
                        break;
                    case "能否打孔":
                        chineseConverted["CanBePunched"] = chineseProp.Value.Value<bool>();
                        break;
                    case "打孔职业":
                        chineseConverted["PunchRace"] = ConvertObjectRace(chineseProp.Value.Value<string>());
                        break;
                    case "打孔上限":
                        chineseConverted["PunchCap"] = chineseProp.Value.Value<int>();
                        break;
                    case "一孔花费":
                        chineseConverted["OneHoleCost"] = chineseProp.Value.Value<int>();
                        break;
                    case "二孔花费":
                        chineseConverted["TwoHoleCost"] = chineseProp.Value.Value<int>();
                        break;
                    case "重铸灵石":
                        chineseConverted["ReforgeSpiritStone"] = chineseProp.Value.Value<int>();
                        break;
                    case "灵石数量":
                        chineseConverted["SpiritStonesAmount"] = chineseProp.Value.Value<int>();
                        break;
                    case "金币数量":
                        chineseConverted["CoinAmount"] = chineseProp.Value.Value<int>();
                        break;
                    case "装备特技":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<string>();
                        break;
                    case "装备套装":
                        chineseConverted["EquipSet"] = ConvertGameItemSet(chineseProp.Value.Value<string>());
                        break;
                    case "最小神圣伤害":
                        chineseConverted["MinHC"] = chineseProp.Value.Value<int>();
                        break;
                    case "最大神圣伤害":
                        chineseConverted["MaxHC"] = chineseProp.Value.Value<int>();
                        break;
                    case "铭文职业":
                        chineseConverted["InscriptionRace"] = ConvertObjectRace(chineseProp.Value.Value<string>());
                        break;
                    case "灵魂绑点":
                        chineseConverted["SoulTiePoints"] = chineseProp.Value.Value<int>();
                        break;
                    case "圣伤上限":
                        chineseConverted["HolyDamageLimit"] = chineseProp.Value.Value<int>();
                        break;


                    // Not used yet
                    case "勇者试炼":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<bool>();
                        break;

                    case "试炼币数":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
                        break;

                    case "铭文传承":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<bool>();
                        break;

                    case "最大神圣等级":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
                        break;

                    case "灵魂绑定点数":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
                        break;

                    case "邮寄价格":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
                        break;

                    case "铭文继承":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<bool>();
                        break;

                    case "附加技能1":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<string>();
                        break;

                    case "铭文刻印":
                        chineseConverted[chineseProp.Key] = chineseProp.Value.Value<bool>();
                        break;

                    default: throw new ApplicationException();
                }
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseModel.Value<int>("物品编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("ID") ? (int)chineseConverted["ID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpItemSets()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "套装数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "Sets");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "装备套装编号":
                    chineseConverted["SetID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "套装名称":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "游戏装备套装属性":
                    chineseConverted["Stats"] = ConvertStats(chineseProp.Value.Values<JProperty>());
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("SetID") == chineseModel.Value<int>("装备套装编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("SetID") ? (ushort)chineseConverted["SetID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpEquipmentStats()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "物品数据", "装备属性");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "EquipmentStats");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "装备部位":
                    chineseConverted["ItemType"] = ConvertItemType(chineseProp.Value.Value<string>());
                    break;
                case "极品概率":
                    chineseConverted["MaxProbability"] = chineseProp.Value.Value<float>();
                    break;
                case "单条概率":
                    chineseConverted["MinRate"] = chineseProp.Value.Value<int>();
                    break;
                case "两条概率":
                    chineseConverted["MaxRate"] = chineseProp.Value.Value<int>();
                    break;
                case "属性列表":
                    chineseConverted["Stats"] = chineseProp.Value.Values<JObject>().Select(x => ConvertEquipmentStats(x)).ToArray();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<string>("ItemType") == chineseModel.Value<string>("装备部位"));

        var name = chineseConverted["ItemType"];
        var fileName = $"{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpRareTreasureItems()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "物品数据", "珍宝商品");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "Treasures");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<(string, JObject)>();
    var mir3DbModels = new List<(string, JObject)>();

    foreach (var fileName in chineseFiles)
    {
        var content = File.ReadAllText(fileName);
        chineseModels.Add((fileName, (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions)));
    }

    foreach (var fileName in mir3DbFiles)
    {
        var content = File.ReadAllText(fileName);
        mir3DbModels.Add((fileName, (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions)));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel.Item2)
        {
            switch (chineseProp.Key)
            {
                case "物品编号":
                    chineseConverted["ItemID"] = chineseProp.Value.Value<int>();
                    break;
                case "单位数量":
                    chineseConverted["UnitCount"] = chineseProp.Value.Value<int>();
                    break;
                case "商品分类":
                    chineseConverted["Type"] = chineseProp.Value.Value<byte>();
                    break;
                case "商品标签":
                    chineseConverted["Label"] = chineseProp.Value.Value<byte>();
                    break;
                case "补充参数":
                    chineseConverted["AdditionalParam"] = chineseProp.Value.Value<byte>();
                    break;
                case "商品原价":
                    chineseConverted["OriginalPrice"] = chineseProp.Value.Value<int>();
                    break;
                case "商品现价":
                    chineseConverted["CurrentPrice"] = chineseProp.Value.Value<int>();
                    break;
                case "买入绑定":
                    chineseConverted["BuyBound"] = chineseProp.Value.Value<byte>();
                    break;
                case "Uid":
                    chineseConverted["UID"] = chineseProp.Value.Value<int>();
                    break;
                case "珍宝阁排版":
                    chineseConverted[chineseProp.Key] = chineseProp.Value.Value<int>();
                    break;
                case "StartTime":
                    chineseConverted[chineseProp.Key] = chineseProp.Value.Value<DateTime>();
                    break;
                case "EndTime":
                    chineseConverted[chineseProp.Key] = chineseProp.Value.Value<DateTime>();
                    break;
                case "事件":
                    chineseConverted[chineseProp.Key] = chineseProp.Value.Value<byte>();
                    break;
                case "类别":
                    chineseConverted[chineseProp.Key] = chineseProp.Value.Value<byte>();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FindIndex(x => x.Item2.Value<int>("ItemID") == chineseModel.Item2.Value<int>("物品编号"));

        var fileName = chineseModel.Item1;

        var mir3dOutputPath = Path.Combine(mir3DbFolder, Path.GetFileName(fileName));

        if (mir3dModel == -1)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpGameStores()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "物品数据", "游戏商店");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "GameStore");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "商店编号":
                    chineseConverted["StoreID"] = chineseProp.Value.Value<int>();
                    break;
                case "商店名字":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "回收类型":
                    chineseConverted["StoreType"] = ConvertStoreType(chineseProp.Value.Value<string>());
                    break;
                case "商品列表":
                    chineseConverted["Products"] = chineseProp.Value.Values<JObject>().Select(x => ConvertStoreProducts(x)).ToArray();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("StoreID") == chineseModel.Value<int>("商店编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("StoreID") ? (int)chineseConverted["StoreID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMonsters()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "Npc数据", "怪物数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Npc", "Monsters");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "怪物名字":
                    chineseConverted["MonsterName"] = chineseProp.Value.Value<string>();
                    break;
                case "怪物编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "怪物等级":
                    chineseConverted["Level"] = chineseProp.Value.Value<byte>();
                    break;
                case "怪物体型":
                    chineseConverted["Size"] = ConvertObjectSize(chineseProp.Value.Value<string>());
                    break;
                case "怪物分类":
                    chineseConverted["Race"] = ConvertMonsterRace(chineseProp.Value.Value<string>());
                    break;
                case "怪物级别":
                    chineseConverted["Grade"] = ConvertMonsterGrade(chineseProp.Value.Value<string>());
                    break;
                case "怪物禁止移动":
                    chineseConverted["ForbbidenMove"] = chineseProp.Value.Value<bool>();
                    break;
                case "脱战自动石化":
                    chineseConverted["OutWarAutomaticPetrochemical"] = chineseProp.Value.Value<bool>();
                    break;
                case "石化状态编号":
                    chineseConverted["PetrochemicalStatusID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "可见隐身目标":
                    chineseConverted["VisibleStealthTargets"] = chineseProp.Value.Value<bool>();
                    break;
                case "可被技能推动":
                    chineseConverted["CanBeDrivenBySkills"] = chineseProp.Value.Value<bool>();
                    break;
                case "可被技能控制":
                    chineseConverted["CanBeControlledBySkills"] = chineseProp.Value.Value<bool>();
                    break;
                case "可被技能诱惑":
                    chineseConverted["CanBeSeducedBySkills"] = chineseProp.Value.Value<bool>();
                    break;
                case "可被技能召唤":
                    chineseConverted["CanBeSummonedBySkills"] = chineseProp.Value.Value<bool>();
                    break;
                case "基础诱惑概率":
                    chineseConverted["BaseTemptationProbability"] = chineseProp.Value.Value<float>();
                    break;
                case "怪物移动间隔":
                    chineseConverted["MoveInterval"] = chineseProp.Value.Value<ushort>();
                    break;
                case "怪物漫游间隔":
                    chineseConverted["RoamInterval"] = chineseProp.Value.Value<ushort>();
                    break;
                case "尸体保留时长":
                    chineseConverted["CorpsePreservationDuration"] = chineseProp.Value.Value<ushort>();
                    break;
                case "主动攻击目标":
                    chineseConverted["ActiveAttackTarget"] = chineseProp.Value.Value<bool>();
                    break;
                case "怪物仇恨范围":
                    chineseConverted["RangeHate"] = chineseProp.Value.Value<byte>();
                    break;
                case "怪物仇恨时间":
                    chineseConverted["HateTime"] = chineseProp.Value.Value<ushort>();
                    break;
                case "普通攻击技能":
                    chineseConverted["NormalAttackSkills"] = chineseProp.Value.Value<string>();
                    break;
                case "概率触发技能":
                    chineseConverted["ProbabilityTriggerSkills"] = chineseProp.Value.Value<string>();
                    break;
                case "进入战斗技能":
                    chineseConverted["EnterCombatSkills"] = chineseProp.Value.Value<string>();
                    break;
                case "退出战斗技能":
                    chineseConverted["ExitCombatSkills"] = chineseProp.Value.Value<string>();
                    break;
                case "瞬移释放技能":
                    chineseConverted["MoveReleaseSkill"] = chineseProp.Value.Value<string>();
                    break;
                case "复活释放技能":
                    chineseConverted["BirthReleaseSkill"] = chineseProp.Value.Value<string>();
                    break;
                case "死亡释放技能":
                    chineseConverted["DeathReleaseSkill"] = chineseProp.Value.Value<string>();
                    break;
                case "怪物基础":
                    chineseConverted["Stats"] = chineseProp.Value.Values<JObject>().Select(x => ConvertBasicStats(x)).ToArray();
                    break;
                case "怪物成长":
                    chineseConverted["Grows"] = chineseProp.Value.Values<JObject>().Select(x => ConvertGrowthStat(x)).ToArray();
                    break;
                case "继承属性":
                    chineseConverted["InheritsStats"] = chineseProp.Value.Values<JObject>().Select(x => ConvertInheritStat(x)).ToArray();
                    break;
                case "怪物提供经验":
                    chineseConverted["ProvideExperience"] = chineseProp.Value.Value<ushort>();
                    break;
                case "怪物掉落物品":
                    chineseConverted["Drops"] = chineseProp.Value.Values<JObject>().Select(x => ConvertMonsterDrop(x)).ToArray();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseModel.Value<int>("怪物编号"));

        var name = chineseConverted["MonsterName"];
        var id = chineseConverted.ContainsKey("ID") ? (ushort)chineseConverted["ID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMaps()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "游戏地图", "地图数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "GameMap", "Maps");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "地图编号":
                    chineseConverted["MapID"] = chineseProp.Value.Value<byte>();
                    break;
                case "地图名字":
                    chineseConverted["MapName"] = chineseProp.Value.Value<string>();
                    break;
                case "地图别名":
                    chineseConverted["MapFile"] = chineseProp.Value.Value<string>();
                    break;
                case "地形文件":
                    chineseConverted["TerrainFile"] = chineseProp.Value.Value<string>();
                    break;
                case "限制人数":
                    chineseConverted["LimitPlayers"] = chineseProp.Value.Value<int>();
                    break;
                case "限制等级":
                    chineseConverted["MinLevel"] = chineseProp.Value.Value<byte>();
                    break;
                case "分线数量":
                    chineseConverted["LimitInstances"] = chineseProp.Value.Value<byte>();
                    break;
                case "下线传送":
                    chineseConverted["NoReconnect"] = chineseProp.Value.Value<bool>();
                    break;
                case "传送地图":
                    chineseConverted["NoReconnectMapID"] = chineseProp.Value.Value<byte>();
                    break;
                case "副本地图":
                    chineseConverted["QuestMap"] = chineseProp.Value.Value<bool>();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("MapID") == chineseModel.Value<int>("地图编号"));

        var name = chineseConverted["MapName"];
        var id = chineseConverted.ContainsKey("MapID") ? (byte)chineseConverted["MapID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMapAreas()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "游戏地图", "地图区域");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "GameMap", "MapAreas");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "所处地图":
                    chineseConverted["MapID"] = chineseProp.Value.Value<byte>();
                    break;
                case "所处地名":
                    chineseConverted["MapName"] = chineseProp.Value.Value<string>();
                    break;
                case "所处坐标":
                    chineseConverted["Coordinates"] = chineseProp.Value.Value<string>();
                    break;
                case "区域名字":
                    chineseConverted["RegionName"] = chineseProp.Value.Value<string>();
                    break;
                case "区域半径":
                    chineseConverted["AreaRadius"] = chineseProp.Value.Value<int>();
                    break;
                case "区域类型":
                    chineseConverted["RegionType"] = ConvertAreaType(chineseProp.Value.Value<string>());
                    break;
                case "范围坐标":
                    chineseConverted["RangeCoordinates"] = chineseProp.Value;
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("MapID") == chineseModel.Value<int>("所处地图"));

        var name = chineseConverted["RegionName"];
        var id = chineseConverted.ContainsKey("MapID") ? (byte)chineseConverted["MapID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMapSpawns()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "游戏地图", "怪物刷新");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "GameMap", "Monsters");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "所处地图":
                    chineseConverted["MapID"] = chineseProp.Value.Value<byte>();
                    break;
                case "所处地名":
                    chineseConverted["MapName"] = chineseProp.Value.Value<string>();
                    break;
                case "所处坐标":
                    chineseConverted["Coordinates"] = chineseProp.Value.Value<string>();
                    break;
                case "区域名字":
                    chineseConverted["RegionName"] = chineseProp.Value.Value<string>();
                    break;
                case "区域半径":
                    chineseConverted["AreaRadius"] = chineseProp.Value.Value<int>();
                    break;
                case "刷新列表":
                    chineseConverted["Spawns"] = chineseProp.Value.Values<JObject>().Select(x => ConvertMonsterSpawnInfo(x)).ToArray();
                    break;
                case "范围坐标":
                    chineseConverted["RangeCoordinates"] = chineseProp.Value;
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("MapID") == chineseModel.Value<int>("所处地图"));

        var name = chineseConverted["RegionName"];
        var id = chineseConverted.ContainsKey("MapID") ? (byte)chineseConverted["MapID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMapGuards()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "游戏地图", "守卫刷新");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "GameMap", "Guards");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile, System.Text.Encoding.UTF8);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));

    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "守卫编号":
                    chineseConverted["GuardID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "所处地图":
                    chineseConverted["MapID"] = chineseProp.Value.Value<byte>();
                    break;
                case "所处地名":
                    chineseConverted["MapName"] = chineseProp.Value.Value<string>();
                    break;
                case "所处坐标":
                    chineseConverted["Coordinates"] = chineseProp.Value.Value<string>();
                    break;
                case "所处方向":
                    chineseConverted["Direction"] = ConvertGameDirection(chineseProp.Value.Value<string>());
                    break;
                case "区域名字":
                    chineseConverted["RegionName"] = chineseProp.Value.Value<string>();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("MapID") == chineseModel.Value<int>("所处地图"));

        var name = chineseConverted["RegionName"];
        var id = chineseConverted.ContainsKey("MapID") ? (byte)chineseConverted["MapID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMapTeleports()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "游戏地图", "法阵数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "GameMap", "TeleportGates");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile, System.Text.Encoding.UTF8);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));

    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "法阵编号":
                    chineseConverted["GateID"] = chineseProp.Value.Value<byte>();
                    break;
                case "所处地图":
                    chineseConverted["MapID"] = chineseProp.Value.Value<byte>();
                    break;
                case "跳转地图":
                    chineseConverted["ToMapID"] = chineseProp.Value.Value<byte>();
                    break;
                case "法阵名字":
                    chineseConverted["GateName"] = chineseProp.Value.Value<string>();
                    break;
                case "所处地名":
                    chineseConverted["MapName"] = chineseProp.Value.Value<string>();
                    break;
                case "跳转地名":
                    chineseConverted["ToMapName"] = chineseProp.Value.Value<string>();
                    break;
                case "所处别名":
                    chineseConverted["MapFileName"] = chineseProp.Value.Value<string>();
                    break;
                case "跳转别名":
                    chineseConverted["ToMapFileName"] = chineseProp.Value.Value<string>();
                    break;
                case "所处坐标":
                    chineseConverted["Coordinates"] = chineseProp.Value.Value<string>();
                    break;
                case "跳转坐标":
                    chineseConverted["ToCoordinates"] = chineseProp.Value.Value<string>();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("GateID") == chineseModel.Value<int>("法阵编号") &&
            x.Value<int>("MapID") == chineseModel.Value<int>("所处地图") && x.Value<string>("GateName") == chineseModel.Value<string>("法阵名字"));

        var sname = chineseConverted["MapName"];
        var dname = chineseConverted["GateName"];
        var sid = chineseConverted.ContainsKey("MapID") ? (byte)chineseConverted["MapID"] : 0;
        var did = chineseConverted.ContainsKey("GateID") ? (byte)chineseConverted["GateID"] : 0;
        var fileName = $"{sid}-{sname}-{did}-{dname}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMounts()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "坐骑数据", "骑马坐骑");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Mounts", "RidingMounts");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "坐骑编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "坐骑名字":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "AuraID":
                    chineseConverted["AuraID"] = chineseProp.Value.Value<int>();
                    break;
                case "MountPower":
                    chineseConverted["MountPower"] = chineseProp.Value.Value<short>();
                    break;
                case "Buff编号":
                    chineseConverted["BuffID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "品质":
                    chineseConverted["Quality"] = chineseProp.Value.Value<byte>();
                    break;
                case "等级限制":
                    chineseConverted["LevelLimit"] = chineseProp.Value.Value<byte>();
                    break;
                case "速度修改率":
                    chineseConverted["SpeedModificationRate"] = chineseProp.Value.Value<int>();
                    break;
                case "HitUnmountRate":
                    chineseConverted["HitUnmountRate"] = chineseProp.Value.Value<int>();
                    break;
                case "坐骑属性":
                    chineseConverted["Stats"] = ConvertStats(chineseProp.Value.Values<JProperty>());
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseModel.Value<int>("坐骑编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("ID") ? (ushort)chineseConverted["ID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpMountBeasts()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "坐骑数据", "坐骑御兽");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Mounts", "MountBeast");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "坐骑编号":
                    chineseConverted["MountID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "坐骑名字":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "坐骑御兽属性":
                    chineseConverted["Stats"] = ConvertStats(chineseProp.Value.Values<JProperty>());
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("MountID") == chineseModel.Value<int>("坐骑编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("MountID") ? (ushort)chineseConverted["MountID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpGameTitles()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "物品数据", "游戏称号");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "GameTitle");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "称号编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<byte>();
                    break;
                case "称号名字":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "称号战力":
                    chineseConverted["CombatPower"] = chineseProp.Value.Value<int>();
                    break;
                case "有效时间":
                    chineseConverted["EffectiveTime"] = chineseProp.Value.Value<int>();
                    break;
                case "称号属性":
                    chineseConverted["Stats"] = ConvertStats(chineseProp.Value.Values<JProperty>());
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseModel.Value<int>("坐骑编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("ID") ? (byte)chineseConverted["ID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpGuards()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "Npc数据", "守卫数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Npc", "Guards");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile, System.Text.Encoding.UTF8);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "守卫名字":
                    chineseConverted["Name"] = chineseProp.Value.Value<string>();
                    break;
                case "守卫编号":
                    chineseConverted["GuardID"] = chineseProp.Value.Value<ushort>();
                    break;
                case "守卫等级":
                    chineseConverted["Level"] = chineseProp.Value.Value<byte>();
                    break;
                case "虚无状态":
                    chineseConverted["Nothingness"] = chineseProp.Value.Value<bool>();
                    break;
                case "能否受伤":
                    chineseConverted["CanBeInjured"] = chineseProp.Value.Value<bool>();
                    break;
                case "尸体保留":
                    chineseConverted["CorpsePreservation"] = chineseProp.Value.Value<int>();
                    break;
                case "复活间隔":
                    chineseConverted["RevivalInterval"] = chineseProp.Value.Value<int>();
                    break;
                case "主动攻击":
                    chineseConverted["ActiveAttack"] = chineseProp.Value.Value<bool>();
                    break;
                case "仇恨范围":
                    chineseConverted["RangeHate"] = chineseProp.Value.Value<byte>();
                    break;
                case "普攻技能":
                    chineseConverted["BasicAttackSkills"] = chineseProp.Value.Value<string>();
                    break;
                case "商店编号":
                    chineseConverted["StoreID"] = chineseProp.Value.Value<int>();
                    break;
                case "界面代码":
                    chineseConverted["InterfaceCode"] = chineseProp.Value.Value<string>();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("GuardID") == chineseModel.Value<int>("守卫编号"));

        var name = chineseConverted["Name"];
        var id = chineseConverted.ContainsKey("GuardID") ? (ushort)chineseConverted["GuardID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpNPCDialogs()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "Npc数据", "对话数据");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Npc", "Dialogs");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile, System.Text.Encoding.UTF8);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "对话编号":
                    chineseConverted["ID"] = chineseProp.Value.Value<int>();
                    break;
                case "对话内容":
                    chineseConverted["Content"] = chineseProp.Value.Value<string>();
                    break;
                

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("ID") == chineseModel.Value<int>("对话编号"));

        var id = chineseConverted.ContainsKey("ID") ? (int)chineseConverted["ID"] : 0;
        var fileName = $"{id}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

void DumpItemRandomStats()
{
    var chineseFolder = Path.Combine(chineseDbSystemPath, "物品数据", "随机属性");
    var mir3DbFolder = Path.Combine(mir3dDbSystemPath, "Items", "RandomStats");

    var chineseFiles = Directory.GetFiles(chineseFolder, "*.txt", SearchOption.TopDirectoryOnly);
    var mir3DbFiles = Directory.GetFiles(mir3DbFolder, "*.txt", SearchOption.TopDirectoryOnly);

    var chineseModels = new List<JObject>();
    var mir3DbModels = new List<JObject>();

    foreach (var chineseFile in chineseFiles)
    {
        var content = File.ReadAllText(chineseFile, System.Text.Encoding.UTF8);
        chineseModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));

    }

    foreach (var mir3dFile in mir3DbFiles)
    {
        var content = File.ReadAllText(mir3dFile);
        mir3DbModels.Add((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content, jsonOptions));
    }

    foreach (var chineseModel in chineseModels)
    {
        var chineseConverted = new Dictionary<string, object>();

        foreach (var chineseProp in chineseModel)
        {
            switch (chineseProp.Key)
            {
                case "对应属性":
                    chineseConverted["Stat"] = ConvertStat(chineseProp.Value.Value<string>());
                    break;
                case "属性数值":
                    chineseConverted["Value"] = chineseProp.Value.Value<int>();
                    break;
                case "属性编号":
                    chineseConverted["StatID"] = chineseProp.Value.Value<int>();
                    break;
                case "战力加成":
                    chineseConverted["CombatBonus"] = chineseProp.Value.Value<int>();
                    break;
                case "属性描述":
                    chineseConverted["StatDescription"] = chineseProp.Value.Value<string>();
                    break;

                default: throw new ApplicationException();
            }
        }

        var mir3dModel = mir3DbModels.FirstOrDefault(x => x.Value<int>("StatID") == chineseModel.Value<int>("属性编号"));

        var name = chineseConverted["StatDescription"];
        var id = chineseConverted.ContainsKey("StatID") ? (int)chineseConverted["StatID"] : 0;
        var fileName = $"{id}-{name}.txt";

        var mir3dOutputPath = Path.Combine(mir3DbFolder, fileName);

        if (mir3dModel == null)
        {
            var content = JsonConvert.SerializeObject(chineseConverted, jsonOptions);
            File.WriteAllText(mir3dOutputPath, content);
        }
    }
}

bool CompareRecursiveValue(object source, JToken target, string[] omitFields)
{
    var modified = false;

    switch (target.Type)
    {
        case JTokenType.String:
            if ((string)((JValue)target).Value != (string)source)
            {
                modified = true;
                ((JValue)target).Value = source;
            }
            break;
        case JTokenType.Boolean:
            if ((bool)((JValue)target).Value != (bool)source)
            {
                modified = true;
                ((JValue)target).Value = source;
            }
            break;
        case JTokenType.Float:
            if ((float)((JValue)target).Value != (float)source)
            {
                modified = true;
                ((JValue)target).Value = source;
            }
            break;
        case JTokenType.Integer:
            var targetInt = ((JValue)target).Value != null && ((JValue)target).Value is int ? new int?((int)((JValue)target).Value) : null;
            var targetLong = ((JValue)target).Value != null && ((JValue)target).Value is long ? new long?((long)((JValue)target).Value) : null;

            var sourceInt = source != null && source is int ? new int?((int)source) : null;
            var sourceLong = source != null && source is long ? new long?((long)source) : null;

            if (targetInt == null && targetLong == null) return false;
            else if (sourceInt == null && sourceLong == null) return false;

            var targetNum = targetLong != null ? targetLong : (int)targetInt;
            var sourceNum = sourceLong != null ? sourceLong : (int)sourceInt;

            if (targetNum != sourceNum)
            {
                modified = true;
                ((JValue)target).Value = source;
            }
            break;
        case JTokenType.Object:
            if (source is not IDictionary sourceObj)
                throw new ApplicationException();

            var keys = sourceObj.Keys.Cast<string>().ToArray();

            foreach (var prop in (JObject)target)
            {
                if (!omitFields.Contains(prop.Key) && keys.Contains(prop.Key) && CompareRecursiveValue(sourceObj[prop.Key], prop.Value, omitFields))
                    modified = true;
            }
            break;
        case JTokenType.Array:
            if (source is not IEnumerable collection) throw new ApplicationException();
            var arr = collection.Cast<object>().ToArray();

            for (var i = 0; i < arr.Length; i++)
            {
                var sourceItem = arr[i];
                var targetItem = ((JArray)target)[i];
                if (CompareRecursiveValue(sourceItem, targetItem, omitFields))
                    modified = true;
            }
            break;
        default:
            throw new NotImplementedException();
    }

    return modified;
}

Dictionary<string, object> ConvertTreasureItem(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "物品名":
                output.Add("ItemName", prop.Value.Value<string>());
                break;
            case "职业":
                output.Add("NeedRace", ConvertObjectRace(prop.Value.Value<string>()));
                break;
            case "概率":
                output.Add("Rate", prop.Value.Value<int>());
                break;
            default:
                throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertStoreProducts(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "商品编号":
                output.Add("ItemID", prop.Value.Value<int>());
                break;
            case "单位数量":
                output.Add("Quantity", prop.Value.Value<int>());
                break;
            case "货币类型":
                output.Add("CurrencyModel", prop.Value.Value<int>());
                break;
            case "商品价格":
                output.Add("Price", prop.Value.Value<int>());
                break;

            default: throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertSkillNode(JObject obj)
{
    var output = new Dictionary<string, object>();

    var skillName = string.Empty;
    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "$type":
                skillName = prop.Value.Value<string>();
                output.Add("$type", ConvertSkillNodeType(prop.Value.Value<string>()));
                break;
        }
    }

    if (string.IsNullOrEmpty(skillName))
        throw new ApplicationException();

    switch (skillName)
    {
        case "A_00_触发子类技能, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "技能触发方式":
                            output.Add(prop.Key, ConvertSkillTriggerMethod(prop.Value.Value<string>()));
                            break;
                        case "触发技能名字":
                            output.Add("触发SkillName", prop.Value.Value<string>());
                            break;
                        case "反手技能名字":
                            output.Add("反手SkillName", prop.Value.Value<string>());
                            break;
                        case "计算触发概率":
                            output.Add("CalculateTriggerProbability", prop.Value.Value<bool>());
                            break;
                        case "计算幸运概率":
                            output.Add("CalculateLuckyProbability", prop.Value.Value<bool>());
                            break;
                        case "技能触发概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "增加概率Buff":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "Buff增加系数":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "验证自身Buff":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "自身Buff编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "触发成功移除":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "验证铭文技能":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "所需铭文编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "同组铭文无效":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "A_01_触发对象Buff, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "角色自身添加":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "触发Buff编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "伴生Buff编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "Buff触发概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "验证铭文技能":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "所需铭文编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "同组铭文无效":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "验证自身Buff":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "自身Buff编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "触发成功移除":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "移除伴生Buff":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "移除伴生编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "验证分组Buff":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "Buff分组编号":
                            output.Add("BuffGroupID", prop.Value.Value<ushort>());
                            break;
                        case "验证目标Buff":
                            output.Add("VerifyTargetBuff", prop.Value.Value<bool>());
                            break;
                        case "目标Buff编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "所需Buff层数":
                            output.Add(prop.Key, prop.Value.Value<byte>());
                            break;
                        case "验证目标类型":
                            output.Add("VerifyTargetType", prop.Value.Value<bool>());
                            break;
                        case "所需目标类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<ushort>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "A_02_触发陷阱技能, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "技能触发方式":
                            output.Add(prop.Key, ConvertSkillTriggerMethod(prop.Value.Value<string>()));
                            break;
                        case "触发陷阱技能":
                            output.Add("TriggerTrapSkills", prop.Value.Value<string>());
                            break;
                        case "触发陷阱数量":
                            output.Add("NumberTrapsTriggered", ConvertObjectSize(prop.Value.Value<string>()));
                            break;
                        case "陷阱间距":
                            output.Add("TrapSpacing", prop.Value.Value<int>());
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<ushort>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "B_00_技能切换通知, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "技能标记编号":
                            output.Add("SkillTagID", prop.Value.Value<ushort>());
                            break;
                        case "允许移除标记":
                            output.Add("TagRemovalAllowed", prop.Value.Value<bool>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "B_01_技能释放通知, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "发送释放通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "移除技能标记":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "调整角色朝向":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "自身冷却时间":
                            output.Add("SelfCooldown", prop.Value.Value<int>());
                            break;
                        case "Buff增加冷却":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "增加冷却Buff":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "冷却增加时间":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "分组冷却时间":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "角色忙绿时间":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<ushort>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "B_02_技能命中通知, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "命中扩展通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "计算飞行耗时":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "单格飞行耗时":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "B_03_前摇结束通知, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "发送结束通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "计算攻速缩减":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "角色硬直时间":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "禁止行走时间":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "禁止奔跑时间":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "解除技能陷阱":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "B_04_后摇结束通知, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "发送结束通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "后摇结束死亡":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_00_计算技能锚点, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "计算当前位置":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "计算当前方向":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "技能最远距离":
                            output.Add("MaxDistance", prop.Value.Value<int>());
                            break;
                        case "技能最近距离":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_01_计算命中目标, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "清空命中列表":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "技能能否穿墙":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "技能能否招架":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "技能锁定方式":
                            output.Add(prop.Key, ConvertSkillLockType(prop.Value.Value<string>()));
                            break;
                        case "技能闪避方式":
                            output.Add("SkillEvasion", ConvertSkillEvasionType(prop.Value.Value<string>()));
                            break;
                        case "技能命中反馈":
                            output.Add("SkillHitFeedback", ConvertSkillHitFeedback(prop.Value.Value<string>()));
                            break;
                        case "技能范围类型":
                            output.Add(prop.Key, ConvertObjectSize(prop.Value.Value<string>()));
                            break;
                        case "放空结束技能":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "发送中断通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "补发释放通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "技能命中通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "技能扩展通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "计算飞行耗时":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "单格飞行耗时":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "限定命中数量":
                            output.Add("HitsLimit", prop.Value.Value<int>());
                            break;
                        case "限定目标类型":
                            output.Add("LimitTargetType", ConvertObjectType(prop.Value.Value<string>()));
                            break;
                        case "限定目标关系":
                            output.Add("LimitTargetRelationship", ConvertLimitedTargetRelationship(prop.Value.Value<string>()));
                            break;
                        case "限定特定类型":
                            output.Add("LimitSpecificType", ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "攻速提升类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "攻速提升幅度":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "触发被动技能":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "触发被动概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<ushort>());
                            break;
                        case "清除目标状态":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "清除状态列表":
                            output.Add(prop.Key, prop.Value.Values<ushort>().ToList());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_02_计算目标伤害, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "点爆命中目标":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "点爆标记编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "点爆需要层数":
                            output.Add(prop.Key, prop.Value.Value<byte>());
                            break;
                        case "失败添加层数":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "技能伤害基数":
                            output.Add(prop.Key, prop.Value.Values<int>().ToArray());
                            break;
                        case "技能伤害系数":
                            output.Add(prop.Key, prop.Value.Values<float>().ToArray());
                            break;
                        case "技能伤害类型":
                            output.Add(prop.Key, ConvertSkillDamageType(prop.Value.Value<string>()));
                            break;
                        case "技能增伤类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "技能增伤基数":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "技能增伤系数":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "数量衰减伤害":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "伤害衰减系数":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "伤害衰减下限":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "技能斩杀类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "技能斩杀概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "技能破防概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "技能破防基数":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "技能破防系数":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "目标硬直时间":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "目标死亡回复":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "命中反馈回复":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "命中反馈限定类型":
                            output.Add(prop.Key, ConvertSkillHitFeedback(prop.Value.Value<string>()));
                            break;
                        case "回复限定类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "体力回复基数":
                            output.Add("PhysicalRecoveryBase", prop.Value.Value<int>());
                            break;
                        case "等级差减回复":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "减回复等级差":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "零回复等级差":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "增加宠物仇恨":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "击杀减少冷却":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "命中减少冷却":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "冷却减少类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "冷却减少技能":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "冷却减少分组":
                            output.Add(prop.Key, prop.Value.Value<byte>());
                            break;
                        case "冷却减少时间":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "扣除武器持久":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<ushort>());
                            break;
                        case "清除目标状态":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "清除状态列表":
                            output.Add(prop.Key, prop.Value.Values<ushort>().ToList());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_03_计算对象位移, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "角色自身位移":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "允许超出锚点":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "锚点反向位移":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "位移增加经验":
                            output.Add("DisplacementIncreaseExp", prop.Value.Value<bool>());
                            break;
                        case "多段位移通知":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "能否穿越障碍":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "自身位移耗时":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "自身硬直时间":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "自身位移次数":
                            output.Add(prop.Key, prop.Value.Value<string>());
                            //output.Add(prop.Key, prop.Value.Values<byte>().ToArray());
                            break;
                        case "自身位移距离":
                            output.Add(prop.Key, prop.Value.Value<string>());
                            //output.Add(prop.Key, prop.Value.Values<byte>().ToArray());
                            break;
                        case "成功Buff编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "成功Buff概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "失败Buff编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "失败Buff概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "推动目标位移":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "推动增加经验":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "推动目标概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "推动目标类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "连续推动数量":
                            output.Add(prop.Key, prop.Value.Value<byte>());
                            break;
                        case "目标位移耗时":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "目标位移距离":
                            output.Add(prop.Key, prop.Value.Value<string>());
                            //output.Add(prop.Key, prop.Value.Values<byte>().ToArray());
                            break;
                        case "目标硬直时间":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "目标位移编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "位移Buff概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "目标附加编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "限定附加类型":
                            output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                            break;
                        case "附加Buff概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "限定锚点距离":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_04_计算目标诱惑, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "检查铭文技能":
                            output.Add(prop.Key, prop.Value.Value<bool>());
                            break;
                        case "检查铭文编号":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "瘫痪状态编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "狂暴状态编号":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "基础诱惑数量":
                            output.Add(prop.Key, prop.Value.Value<string>());
                            //output.Add(prop.Key, prop.Value.Values<byte>().ToArray());
                            break;
                        case "额外诱惑数量":
                            output.Add(prop.Key, prop.Value.Value<byte>());
                            break;
                        case "额外诱惑时长":
                            output.Add(prop.Key, prop.Value.Value<int>());
                            break;
                        case "额外诱惑概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;
                        case "初始宠物等级":
                            output.Add(prop.Key, prop.Value.Value<string>());
                            //output.Add(prop.Key, prop.Value.Values<byte>().ToArray());
                            break;
                        case "特定诱惑列表":
                            output.Add(prop.Key, prop.Value.Values<string>().ToArray());
                            break;
                        case "特定诱惑概率":
                            output.Add(prop.Key, prop.Value.Value<float>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_05_计算目标回复, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "体力回复次数":
                            output.Add(prop.Key, prop.Value.Values<int>().ToArray());
                            break;
                        case "道术叠加次数":
                            output.Add(prop.Key, prop.Value.Values<float>().ToArray());
                            break;
                        case "体力回复基数":
                            output.Add("PhysicalRecoveryBase", prop.Value.Value<string>());
                            //output.Add("PhysicalRecoveryBase", prop.Value.Values<byte>().ToArray());
                            break;
                        case "道术叠加基数":
                            output.Add(prop.Key, prop.Value.Values<float>().ToArray());
                            break;
                        case "立即回复基数":
                            output.Add(prop.Key, prop.Value.Values<int>().ToArray());
                            break;
                        case "立即回复系数":
                            output.Add(prop.Key, prop.Value.Values<float>().ToArray());
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<ushort>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_06_计算宠物召唤, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "召唤宠物名字":
                            output.Add("PetName", prop.Value.Value<string>());
                            break;
                        case "怪物召唤同伴":
                            output.Add("Companion", prop.Value.Value<bool>());
                            break;
                        case "召唤宠物数量":
                            output.Add("SpawnCount", prop.Value.Value<string>());
                            break;
                        case "宠物等级上限":
                            output.Add("LevelCap", prop.Value.Value<string>());
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<int>());
                            break;
                        case "宠物绑定武器":
                            output.Add("PetBoundWeapons", prop.Value.Value<bool>());
                            break;
                        case "检查技能铭文":
                            output.Add("CheckSkillInscriptions", prop.Value.Value<bool>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        case "C_07_计算目标瞬移, Assembly-CSharp":
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key)
                    {
                        case "$type": break;
                        case "每级成功概率":
                            output.Add(prop.Key, prop.Value.Values<float>().ToArray());
                            break;
                        case "瞬移失败提示":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "失败添加Buff":
                            output.Add(prop.Key, prop.Value.Value<ushort>());
                            break;
                        case "增加技能经验":
                            output.Add("GainSkillExp", prop.Value.Value<bool>());
                            break;
                        case "经验技能编号":
                            output.Add("ExpSkillID", prop.Value.Value<ushort>());
                            break;

                        default: throw new ApplicationException();
                    }
                }

                break;
            }

        default: throw new ApplicationException();
    }

    /*foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "$type":
                output.Add("$type", ConvertSkillNodeType(prop.Value.Value<string>()));
                break;



            case "限定命中数量":
                output.Add("HitsLimit", prop.Value.Value<int>());
                break;
            case "限定目标类型":
                output.Add("LimitedTargetType", ConvertObjectType(prop.Value.Value<string>()));
                break;
            case "限定目标关系":
                output.Add("LimitedTargetRelationship", ConvertLimitedTargetRelationship(prop.Value.Value<string>()));
                break;
            case "清除状态列表":
                output.Add(prop.Key, prop.Value.Values<ushort>().ToList());
                break;
            case "技能锁定方式":
                output.Add("技能锁定方式", ConvertSkillLockType(prop.Value.Value<string>()));
                break;
            case "技能闪避方式":
                output.Add("SkillEvasion", ConvertSkillEvasion(prop.Value.Value<string>()));
                break;
            case "技能范围类型":
                output.Add("技能范围类型", ConvertObjectSize(prop.Value.Value<string>()));
                break;
            case "技能命中反馈":
                output.Add("SkillHitFeedback", ConvertSkillHitFeedback(prop.Value.Value<string>()));
                break;
            case "技能伤害类型":
                output.Add(prop.Key, ConvertSkillDamageType(prop.Value.Value<string>()));
                break;
            case "触发Buff编号":
                output.Add("触发Id", prop.Value.Value<int>());
                break;
            case "技能最远距离":
                output.Add("MaxDistance", prop.Value.Value<int>());
                break;
            case "触发被动技能":
                output.Add("触发PassiveSkill", prop.Value.Value<bool>());
                break;
            case "经验技能编号":
                output.Add("ExpSkillId", prop.Value.Value<int>());
                break;
            case "增加技能经验":
                output.Add("GainSkillExp", prop.Value.Value<bool>());
                break;
            case "触发技能名字":
                output.Add("触发SkillName", prop.Value.Value<string>());
                break;
            case "角色自身添加":
                output.Add("角色ItSelf添加", prop.Value.Value<bool>());
                break;
            case "体力回复基数":
                if (prop.Value.Type == JTokenType.String)
                    output.Add("PhysicalRecoveryBase", prop.Value.Value<string>());
                else
                    output.Add("PhysicalRecoveryBase", prop.Value.Value<int>());
                break;
            case "技能增伤类型":
            case "所需目标类型":
            case "回复限定类型":
            case "推动目标类型":
            case "限定附加类型":
            case "攻速提升类型":
            case "技能斩杀类型":
            case "冷却减少类型":
                output.Add(prop.Key, ConvertSpecifyTargetType(prop.Value.Value<string>()));
                break;
            case "限定特定类型":
                output.Add("QualifySpecificType", ConvertSpecifyTargetType(prop.Value.Value<string>()));
                break;
            case "技能触发方式":
                output.Add(prop.Key, ConvertSkillTriggerMethod(prop.Value.Value<string>()));
                break;
            case "技能标记编号":
                output.Add("SkillTagId", prop.Value.Value<int>());
                break;
            case "自身冷却时间":
                output.Add("ItSelfCooldown", prop.Value.Value<int>());
                break;
            case "角色自身位移":
                output.Add("角色ItSelf位移", prop.Value.Value<bool>());
                break;
            case "推动增加经验":
                output.Add("DisplacementIncreaseExp", prop.Value.Value<bool>());
                break;
            case "所需铭文编号":
                output.Add("所需Id", prop.Value.Value<int>());
                break;
            case "自身位移耗时":
                output.Add("ItSelf位移耗时", prop.Value.Value<int>());
                break;
            case "自身位移次数":
                output.Add("ItSelf位移次数", prop.Value.Value<string>());
                break;
            case "自身位移距离":
                output.Add("ItSelf位移距离", prop.Value.Value<string>());
                break;
            case "特定诱惑列表":
                output.Add(prop.Key, prop.Value.Values<string>().ToArray());
                break;
            case "基础诱惑数量":
            case "初始宠物等级":
                output.Add(prop.Key, prop.Value.Value<string>());
                break;
            case "点爆需要层数":
            case "冷却减少分组":
            case "连续推动数量":
                output.Add(prop.Key, prop.Value.Value<byte>());
                break;
            case "所需Buff层数":
            case "技能最近距离":
            case "角色忙绿时间":
            case "目标硬直时间":
            case "角色硬直时间":
            case "禁止行走时间":
            case "禁止奔跑时间":
            case "技能增伤基数":
            case "冷却减少时间":
            case "冷却减少技能":
            case "零回复等级差":
            case "减回复等级差":
            case "技能破防基数":
            case "点爆标记编号":
            case "增加冷却Buff":
            case "冷却增加时间":
            case "目标位移耗时":
            case "目标位移编号":
            case "目标附加编号":
            case "单格飞行耗时":
            case "攻速提升幅度":
            case "增加概率Buff":
            case "瞬移失败提示":
            case "失败添加Buff":
            case "瘫痪状态编号":
            case "狂暴状态编号":
            case "额外诱惑数量":
            case "额外诱惑时长":
                output.Add(prop.Key, prop.Value.Value<int>());
                break;
            case "Buff触发概率":
            case "触发被动概率":
            case "技能触发概率":
            case "Buff增加系数":
            case "技能增伤系数":
            case "技能破防系数":
            case "技能破防概率":
            case "技能斩杀概率":
            case "伤害衰减下限":
            case "伤害衰减系数":
            case "推动目标概率":
            case "位移Buff概率":
            case "附加Buff概率":
            case "失败Buff概率":
            case "成功Buff概率":
            case "额外诱惑概率":
            case "特定诱惑概率":
                output.Add(prop.Key, prop.Value.Value<float>());
                break;
            case "技能伤害基数":
            case "体力回复次数":
            case "立即回复基数":
                output.Add(prop.Key, prop.Value.Values<int>().ToArray());
                break;
            case "目标位移距离":
                output.Add(prop.Key, prop.Value.Value<string>());
                break;
            case "技能伤害系数":
            case "立即回复系数":
            case "每级成功概率":
                output.Add(prop.Key, prop.Value.Values<float>().ToArray());
                break;
            case "失败Buff编号":
                output.Add("失败Id", prop.Value.Value<int>());
                break;
            case "验证目标Buff":
                output.Add("VerifyTargetBuff", prop.Value.Value<bool>());
                break;
            case "放空结束技能":
            case "清除目标状态":
            case "发送释放通知":
            case "调整角色朝向":
            case "技能能否穿墙":
            case "技能能否招架":
            case "技能扩展通知":
            case "扣除武器持久":
            case "发送结束通知":
            case "计算攻速缩减":
            case "解除技能陷阱":
            case "后摇结束死亡":
            case "同组铭文无效":
            case "计算飞行耗时":
            case "验证铭文技能":
            case "触发成功移除":
            case "命中减少冷却":
            case "击杀减少冷却":
            case "增加宠物仇恨":
            case "等级差减回复":
            case "目标死亡回复":
            case "数量衰减伤害":
            case "失败添加层数":
            case "点爆命中目标":
            case "允许移除标记":
            case "移除技能标记":
            case "Buff增加冷却":
            case "允许超出锚点":
            case "锚点反向位移":
            case "多段位移通知":
            case "能否穿越障碍":
            case "推动目标位移":
            case "清空命中列表":
            case "发送中断通知":
            case "补发释放通知":
            case "技能命中通知":
            case "计算当前位置":
            case "计算当前方向":
            case "检查铭文技能":
                output.Add(prop.Key, prop.Value.Value<bool>());
                break;
            case "成功Buff编号":
                output.Add("成功Id", prop.Value.Value<bool>());
                break;
            case "位移增加经验":
                output.Add("DisplacementIncreaseExp", prop.Value.Value<bool>());
                break;
            case "分组冷却时间":
                output.Add("分组Cooldown", prop.Value.Value<int>());
                break;
            case "自身硬直时间":
                output.Add("ItSelf硬直时间", prop.Value.Value<int>());
                break;
            case "目标Buff编号":
                output.Add("目标Id", prop.Value.Value<int>());
                break;
            case "伴生Buff编号":
                output.Add("伴生Id", prop.Value.Value<int>());
                break;
            case "触发陷阱技能":
                output.Add("TriggerTrapSkills", prop.Value.Value<string>());
                break;
            case "触发陷阱数量":
                output.Add("NumberTrapsTriggered", ConvertObjectSize(prop.Value.Value<string>()));
                break;
            case "反手技能名字":
                output.Add("反手SkillName", prop.Value.Value<string>());
                break;
            case "验证自身Buff":
                output.Add("验证ItSelfBuff", prop.Value.Value<bool>());
                break;
            case "自身Buff编号":
                output.Add("Id", prop.Value.Value<int>());
                break;
            case "道术叠加次数":
                output.Add("Taoism叠加次数", prop.Value.Values<float>().ToArray());
                break;
            case "道术叠加基数":
                output.Add("Taoism叠加基数", prop.Value.Values<float>().ToArray());
                break;
            case "计算触发概率":
                output.Add("CalculateTriggerProbability", prop.Value.Value<bool>());
                break;
            case "计算幸运概率":
                output.Add("CalculateLuckyProbability", prop.Value.Value<bool>());
                break;
            case "验证目标类型":
                output.Add("VerifyTargetType", prop.Value.Value<bool>());
                break;
            case "检查铭文编号":
                output.Add("检查Id", prop.Value.Value<int>());
                break;
            case "限定锚点距离": // not exists ignore
                break;
            case "召唤宠物名字":
                output.Add("PetName", prop.Value.Value<string>());
                break;
            case "召唤宠物数量":
                output.Add("SpawnCount", prop.Value.Value<string>());
                break;
            case "宠物等级上限":
                output.Add("LevelCap", prop.Value.Value<string>());
                break;
            case "宠物绑定武器":
                output.Add("PetBoundWeapons", prop.Value.Value<bool>());
                break;
            case "检查技能铭文":
                output.Add("CheckSkillInscriptions", prop.Value.Value<bool>());
                break;
            case "怪物召唤同伴":
                output.Add("Companion", prop.Value.Value<bool>());
                break;
            default:
                throw new ApplicationException();
        }
    }*/

    return output;
}

Dictionary<string, object> ConvertItemProps(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "使用类型":
                output.Add("UsageType", prop.Value.Value<int>());
                break;
            case "恢复时间":
                output.Add("RecoveryTime", prop.Value.Value<int>());
                break;
            case "恢复基数":
                output.Add("RecoveryBase", prop.Value.Value<int>());
                break;
            case "恢复步骤":
                output.Add("RecoverySteps", prop.Value.Value<int>());
                break;
            case "元宝数量":
                output.Add("IngotsAmount", prop.Value.Value<int>());
                break;
            case "宝盒物品概率":
                output.Add("TreasureItemRate", prop.Value.Value<int>());
                break;
            case "双倍经验概率":
                output.Add("DoubleExpRate", prop.Value.Value<int>());
                break;
            case "金币概率":
                output.Add("GoldRate", prop.Value.Value<int>());
                break;
            case "金币数量":
                output.Add("GoldAmount", prop.Value.Value<int>());
                break;
            case "双倍经验":
                output.Add("DoubleExpAmount", prop.Value.Value<int>());
                break;
            case "增加HP":
                output.Add("IncreaseHP", prop.Value.Value<int>());
                break;
            case "增加MP":
                output.Add("IncreaseMP", prop.Value.Value<int>());
                break;
            case "坐骑编号":
                output.Add("MountId", prop.Value.Value<int>());
                break;
            case "地图编号":
                output.Add("MapId", prop.Value.Value<int>());
                break;
            default:
                throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertBasicStats(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "属性":
                output.Add("Stat", ConvertStat(prop.Value.Value<string>()));
                break;
            case "数值":
                output.Add("Value", prop.Value.Value<int>());
                break;

            default: throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertEquipmentStats(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "属性编号":
                output.Add("ID", prop.Value.Value<int>());
                break;
            case "属性概率":
                output.Add("Probability", prop.Value.Value<int>());
                break;
            case "属性描述":
                output.Add("StatDescription", prop.Value.Value<string>());
                break;

            default: throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertGrowthStat(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "属性":
                output.Add("Stat", ConvertStat(prop.Value.Value<string>()));
                break;
            case "零级":
                output.Add("Level0", prop.Value.Value<int>());
                break;
            case "一级":
                output.Add("Level1", prop.Value.Value<int>());
                break;
            case "二级":
                output.Add("Level2", prop.Value.Value<int>());
                break;
            case "三级":
                output.Add("Level3", prop.Value.Value<int>());
                break;
            case "四级":
                output.Add("Level4", prop.Value.Value<int>());
                break;
            case "五级":
                output.Add("Level5", prop.Value.Value<int>());
                break;
            case "六级":
                output.Add("Level6", prop.Value.Value<int>());
                break;
            case "七级":
                output.Add("Level7", prop.Value.Value<int>());
                break;

            default: throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertInheritStat(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "继承属性":
                output.Add("InheritsStats", ConvertStat(prop.Value.Value<string>()));
                break;
            case "转换属性":
                output.Add("ConvertStat", ConvertStat(prop.Value.Value<string>()));
                break;
            case "继承比例":
                output.Add("Ratio", prop.Value.Value<float>());
                break;

            default: throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertMonsterDrop(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "物品名字":
                output.Add("Name", prop.Value.Value<string>());
                break;
            case "怪物名字":
                output.Add("MonsterName", prop.Value.Value<string>());
                break;
            case "爆率分组":
                output.Add(prop.Key, prop.Value.Value<int>());
                break;
            case "掉落概率":
                output.Add("Probability", prop.Value.Value<int>());
                break;
            case "最小数量":
                output.Add("MinAmount", prop.Value.Value<int>());
                break;
            case "最大数量":
                output.Add("MaxAmount", prop.Value.Value<int>());
                break;
            default:
                throw new ApplicationException();
        }
    }

    return output;
}

Dictionary<string, object> ConvertInscriptionSkill(JObject obj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in obj)
    {
        switch (prop.Key)
        {
            case "技能名字":
                output.Add("SkillName", prop.Value.Value<string>());
                break;
            case "技能职业":
                output.Add("Race", ConvertObjectRace(prop.Value.Value<string>()));
                break;
            case "技能编号":
                output.Add("SkillID", prop.Value.Value<ushort>());
                break;
            case "铭文编号":
                output.Add("ID", prop.Value.Value<byte>());
                break;
            case "技能计数":
                output.Add("SkillCount", prop.Value.Value<byte>());
                break;
            case "计数周期":
                output.Add("PeriodCount", prop.Value.Value<ushort>());
                break;
            case "被动技能":
                output.Add("PassiveSkill", prop.Value.Value<bool>());
                break;
            case "铭文品质":
                output.Add("Quality", prop.Value.Value<byte>());
                break;
            case "洗练概率":
                output.Add("Probability", prop.Value.Value<int>());
                break;
            case "广播通知":
                output.Add("BroadcastNotification", prop.Value.Value<bool>());
                break;
            case "铭文描述":
                output.Add("Description", prop.Value.Value<string>());
                break;
            case "需要角色等级":
                output.Add("MinPlayerLevel", prop.Value.Value<string>());
                break;
            case "需要技能经验":
                output.Add("MinSkillExp", prop.Value.Values<int>().ToArray());
                break;
            case "技能战力加成":
                output.Add("SkillCombatBonus", prop.Value.Values<int>().ToArray());
                break;
            case "铭文属性加成":
                output.Add("StatsBonus", prop.Value.Values<JObject>().Select(x => ConvertInscriptionStat(x)).ToArray());
                break;
            case "铭文附带Buff":
                output.Add("ComesWithBuff", prop.Value.Values<int>().ToArray());
                break;
            case "被动技能列表":
                output.Add("PassiveSkills", prop.Value.Values<int>().ToArray());
                break;
            case "主体技能列表":
                output.Add("MainSkills", prop.Value.Values<string>().ToArray());
                break;
            case "开关技能列表":
                output.Add("SwitchSkills", prop.Value.Values<string>().ToArray());
                break;
            case "角色所处状态":
                output.Add(prop.Key, ConvertObjectState(prop.Value.Value<string>()));
                break;
            case "角色死亡消失":
                output.Add("RemoveOnDie", prop.Value.Value<bool>());
                break;

            default:
                throw new ApplicationException();
        }
    }

    return output;
}

string ConvertMonsterRace(string chinese)
{
    switch (chinese)
    {
        case "普通怪物": return "Normal";
        case "不死生物": return "Undead";
        case "虫族生物": return "ZergCreature";
        case "沃玛怪物": return "WomaMonster";
        case "猪类怪物": return "PigMonster";
        case "祖玛怪物": return "ZumaMonster";
        case "魔龙怪物": return "DragonMonster";

        default: throw new ApplicationException();
    }
}

string ConvertMonsterGrade(string chinese)
{
    switch (chinese)
    {
        case "普通怪物": return "Normal";
        case "精英干将": return "Elite";
        case "头目首领": return "Boss";
        case "暗之门怪物": return "DarkGateMonster";

        default: throw new ApplicationException();
    }
}

string ConvertStoreType(string chinese)
{
    switch (chinese)
    {
        case "禁售": return "Ban";
        case "药品": return "Pharmacy";
        case "杂货": return "Groceries";
        case "书籍": return "Book";
        case "武器": return "Weapon";
        case "服装": return "Clothing";
        case "首饰": return "Jewelry";
        case "货币": return "Currency";
        case "宝箱": return "TreasureChest";
        case "坐骑": return "Mounts";

        default: throw new ApplicationException();
    }
}

string ConvertGameItemSet(string chinese)
{
    switch (chinese)
    {
        case "无": return "None";
        default: return chinese;
    }
}

string ConvertPersistentItemType(string chinese)
{
    switch (chinese)
    {
        case "无": return "None";
        case "堆叠": return "Stack";
        default: return chinese;
    }
}

string ConvertItemType(string chinese)
{
    switch (chinese)
    {
        case "无": return "None";
        case "衣服": return "Armour";
        case "披风": return "Cloak";
        case "腰带": return "Belt";
        case "鞋子": return "Boots";
        case "项链": return "Necklace";
        case "戒指": return "Ring";
        case "手镯": return "Bracelet";
        case "护肩": return "ShoulderPad";
        case "武器": return "Weapon";
        case "头盔": return "Helmet";
        case "勋章": return "Medal";
        case "矿石": return "Mineral";

        default: return chinese;
    }
}

string ConvertLimitedTargetRelationship(string chinese)
{
    var values = chinese.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
    var output = new List<string>();
    foreach (var value in values)
    {
        switch (value)
        {
            case "自身": return "Myself";
            case "友方": return "Friendly";
            case "敌对": return "Hostile";
            
            default: throw new ApplicationException();
        }
    }
    return string.Join(", ", output);
}

string ConvertGameObjectRelationship(string chinese)
{
    var values = chinese.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
    var output = new List<string>();
    foreach (var value in values)
    {
        switch (value)
        {
            case "自身": return "Myself";
            case "友方": return "Friendly";
            case "敌对": return "Hostile";

            default: throw new ApplicationException();
        }
    }
    return string.Join(", ", output);
}

string ConvertSpecifyTargetType(string chinese)
{
    var values = chinese.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
    var output = new List<string>();
    foreach (var value in values)
    {
        switch (value)
        {
            case "无": return "None";
            case "低级目标": return "LowLevelTarget";
            case "带盾法师": return "ShieldMage";
            case "低级怪物": return "LowLevelMonster";
            case "低血怪物": return "LowHealthMonster";
            case "普通怪物": return "Normal";
            case "所有怪物": return "AllMonsters";
            case "不死生物": return "Undead";
            case "虫族生物": return "ZergCreature";
            case "沃玛怪物": return "WomaMonster";
            case "猪类怪物": return "PigMonster";
            case "祖玛怪物": return "ZumaMonster";
            case "精英怪物": return "EliteMonsters";
            case "所有宠物": return "AllPets";
            case "背刺目标": return "Backstab";
            case "魔龙怪物": return "DragonMonster";
            case "所有玩家": return "AllPlayers";
            default: throw new ApplicationException();
        }
    }
    return string.Join(", ", output);
}

string ConvertSkillTriggerMethod(string chinese)
{
    switch (chinese)
    {
        case "原点位置绝对触发": return "OriginAbsolutePosition";
        case "锚点位置绝对触发": return "AnchorAbsolutePosition";
        case "刺杀位置绝对触发": return "AssassinationAbsolutePosition";
        case "目标命中绝对触发": return "TargetHitDefinitely";
        case "怪物死亡绝对触发": return "MonsterDeathDefinitely";
        case "怪物死亡换位触发": return "MonsterDeathTransposition";
        case "怪物命中绝对触发": return "MonsterHitDefinitely";
        case "怪物命中概率触发": return "MonsterHitProbability";
        case "无目标锚点位触发": return "NoTargetPosition";
        case "目标位置绝对触发": return "TargetPositionAbsolute";
        case "正手反手随机触发": return "ForehandAndBackhandRandom";
        case "目标死亡绝对触发": return "TargetDeathDefinitely";
        case "目标闪避绝对触发": return "TargetMissDefinitely";

        default: throw new ApplicationException();
    }
}

string ConvertObjectType(string chinese)
{
    if (uint.TryParse(chinese, out var number))
    {
        if (number > 128)
            throw new ApplicationException();
        return chinese;
    }

    var values = chinese.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
    var output = new List<string>();
    foreach (var value in values)
    {
        switch (value)
        {
            case "玩家": output.Add("Player"); break;
            case "宠物": output.Add("Pet"); break;
            case "怪物": output.Add("Monster"); break;
            case "Npcc": output.Add("NPC"); break;
            case "物品": output.Add("Item"); break;
            case "陷阱": output.Add("Trap"); break;
            case "采集": output.Add("Chest"); break;
            default: throw new ApplicationException();
        }
    }
    return string.Join(", ", output);
}

string ConvertSkillNodeType(string chinese)
{
    switch (chinese)
    {
        case "A_00_触发子类技能, Assembly-CSharp":
            return "A_00_TriggerSubSkills, Assembly-CSharp";
        case "A_01_触发对象Buff, Assembly-CSharp":
            return "A_01_TriggerObjectBuff, Assembly-CSharp";
        case "A_02_触发陷阱技能, Assembly-CSharp":
            return "A_02_TriggerTrapSkills, Assembly-CSharp";


        case "B_00_技能切换通知, Assembly-CSharp":
            return "B_00_SkillSwitchNotification, Assembly-CSharp";
        case "B_01_技能释放通知, Assembly-CSharp":
            return "B_01_SkillReleaseNotification, Assembly-CSharp";
        case "B_02_技能命中通知, Assembly-CSharp":
            return "B_02_SkillHitNotification, Assembly-CSharp";
        case "B_03_前摇结束通知, Assembly-CSharp":
            return "B_03_FrontSwingEndNotification, Assembly-CSharp";
        case "B_04_后摇结束通知, Assembly-CSharp":
            return "B_04_BackSwingEndNotification, Assembly-CSharp";


        case "C_00_计算技能锚点, Assembly-CSharp":
            return "C_00_CalculateSkillAnchor, Assembly-CSharp";
        case "C_01_计算命中目标, Assembly-CSharp":
            return "C_01_CalculateHitTarget, Assembly-CSharp";
        case "C_02_计算目标伤害, Assembly-CSharp":
            return "C_02_CalculateTargetDamage, Assembly-CSharp";
        case "C_03_计算对象位移, Assembly-CSharp":
            return "C_03_CalculateObjectDisplacement, Assembly-CSharp";
        case "C_04_计算目标诱惑, Assembly-CSharp":
            return "C_04_CalculateTargetTemptation, Assembly-CSharp";
        case "C_05_计算目标回复, Assembly-CSharp":
            return "C_05_CalculateTargetReply, Assembly-CSharp";
        case "C_06_计算宠物召唤, Assembly-CSharp":
            return "C_06_CalculatePetSummoning, Assembly-CSharp";
        case "C_07_计算目标瞬移, Assembly-CSharp":
            return "C_07_CalculateTargetTeleportation, Assembly-CSharp";
        
        default:
            throw new ApplicationException();
    }
}

string ConvertObjectSize(string chinese)
{
    switch (chinese)
    {
        case "空心3x3": return "Hollow3x3";
        case "空心5x5": return "Hollow5x5";
        case "半月3x1": return "HalfMoon3x1";
        case "半月3x2": return "HalfMoon3x2";
        case "半月3x3": return "HalfMoon3x3";
        case "斩月1x3": return "Zhanyue1x3";
        case "斩月3x3": return "Zhanyue3x3";
        case "螺旋7x7": return "Spiral7x7";
        case "前方3x1": return "Front3x1";
        case "线型1x2": return "LineType1x2";
        case "线型1x5": return "LineType1x5";
        case "线型1x6": return "LineType1x6";
        case "线型1x7": return "LineType1x7";
        case "线型1x8": return "LineType1x8";
        case "线型3x7": return "LineType3x7";
        case "线型3x8": return "LineType3x8";
        case "实心3x3": return "Solid3x3";
        case "实心5x5": return "Solid5x5";
        case "炎龙1x2": return "Yanlong1x2";
        case "菱形3x3": return "Diamond3x3";
        case "单体1x1": return "Single1x1";
        case "螺旋15x15": return "Spiral15x15";
        case "叉型3x3": return "Fork3x3";
        default: throw new ApplicationException();
    }
}

string ConvertSkillEvasionType(string chinese)
{
    switch (chinese)
    {
        case "技能无法闪避": return "SkillCannotBeEvaded";
        case "可被物理闪避": return "CanBePhsyicallyEvaded";
        case "可被魔法闪避": return "CanBeMagicEvaded";
        case "可被中毒闪避": return "CanBePoisonEvaded";
        case "非怪物可闪避": return "NonMonstersCanEvade";

        default: throw new ApplicationException();
    }
}

string ConvertSkillHitFeedback(string chinese)
{
    switch (chinese)
    {
        case "正常": return "Normal";
        case "喷血": return "DamageHealth";
        case "格挡": return "Block";
        case "闪避": return "Miss";
        case "招架": return "Parry";
        case "丢失": return "Lose";
        case "后仰": return "Knockback";
        case "免疫": return "Immune";
        case "死亡": return "Death";
        case "特效": return "SpecialEffect";

        default: throw new ApplicationException();
    }
}

string ConvertSkillLockType(string chinese)
{
    switch (chinese)
    {
        case "锁定自身": return "LockSelf";
        case "锁定目标": return "LockOnTarget";
        case "锁定自身坐标": return "LockOnPosition";
        case "锁定目标坐标": return "LockOnTargetPosition";
        case "锁定锚点坐标": return "LockAnchorPosition";
        case "放空锁定自身": return "EmptyLockSelf";

        default: throw new ApplicationException();
    }
}

string ConvertObjectRace(string chinese)
{
    switch (chinese)
    {
        case "战士": return "Warrior";
        case "法师": return "Wizard";
        case "刺客": return "Assassin";
        case "弓手": return "Archer";
        case "道士": return "Taoist";
        case "龙枪": return "DragonLance";
        case "通用": return "通用";
        case "电脑": return "Demon";
        default: throw new ApplicationException();
    }
}

string ConvertObjectGender(string chinese)
{
    switch (chinese)
    {
        case "不限": return "Any";
        case "男性": return "Man";
        case "女性": return "Woman";
        default: throw new ApplicationException();
    }
}

string ConvertGameDirection(string chinese)
{
    switch (chinese)
    {
        case "左方": return "Left";
        case "左上": return "UpLeft";
        case "上方": return "Up";
        case "右上": return "UpRight";
        case "右方": return "Right";
        case "右下": return "DownRight";
        case "下方": return "Down";
        case "左下": return "DownLeft";

        default: throw new ApplicationException();
    }
}

string ConvertGameSkillType(string chinese)
{
    switch (chinese)
    {
        case "主体技能": return "MainSkills";
        case "子类技能": return "SubSkills";
        default: throw new ApplicationException();
    }
}

string ConvertSkillDamageType(string chinese)
{
    if (UseReadInt)
    {
        if (byte.TryParse(chinese, out var number))
        {
            if (number > 8)
                throw new ApplicationException();
            return chinese;
        }
    }

    switch (chinese)
    {
        case "攻击": return "Attack";
        case "魔法": return "Magic";
        case "道术": return "Taoism";
        case "刺术": return "Piercing";
        case "弓术": return "Archery";
        case "毒性": return "Toxicity";
        case "神圣": return "Sacred";
        case "灼烧": return "Burn";
        case "撕裂": return "Tear";
        default: throw new ApplicationException();
    }
}

string ConvertBuffDetherminationType(string chinese)
{
    if (UseReadInt)
    {
        if (byte.TryParse(chinese, out var number))
        {
            if (number > 128)
                throw new ApplicationException();
            return chinese;
        }
    }

    switch (chinese)
    {
        case "所有技能伤害": return "AllSpellDamage";
        case "所有物理伤害": return "AllPhysicalDamage";
        case "所有魔法伤害": return "AllMagicDamage";
        case "所有特定伤害": return "AllSpecificDamage";
        case "来源技能伤害": return "SourceSkillDamage";
        case "来源物理伤害": return "SourcePhysicalDamage";
        case "来源魔法伤害": return "SourceMagicDamage";
        case "来源特定伤害": return "SourceSpecificDamage";
        default: throw new ApplicationException();
    }
}

string ConvertBuffDetherminationMethod(string chinese)
{
    if (UseReadInt)
    {
        if (byte.TryParse(chinese, out var number))
        {
            if (number > 3)
                throw new ApplicationException();
            return chinese;
        }
    }

    switch (chinese)
    {
        case "主动攻击增伤": return "ActiveAttacksIncreaseDamage";
        case "被动受伤增伤": return "PassiveDamageIncrease";
        case "被动受伤减伤": return "PassiveDecreaseDamage";
        case "主动攻击减伤": return "ActiveAttacksDecreaseDamage";
        default: throw new ApplicationException();
    }
}

string ConvertObjectState(string chinese)
{
    if (UseReadInt)
    {
        if (ushort.TryParse(chinese, out var number))
        {
            if (number > 4096)
                throw new ApplicationException();
            return chinese;
        }
    }

    var values = chinese.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
    var output = new List<string>();
    foreach (var value in values)
    {
        switch (value)
        {
            case "正常状态": return "Normal";
            case "硬直状态": return "Stun";
            case "忙绿状态": return "BusyGreen";
            case "中毒状态": return "Poisoned";
            case "残废状态": return "Disabled";
            case "定身状态": return "Immobilized";
            case "麻痹状态": return "Paralyzed";
            case "霸体状态": return "Hegemony";
            case "无敌状态": return "Invincible";
            case "隐身状态": return "Invisible";
            case "潜行状态": return "Stealth";
            case "失神状态": return "Unconscious";
            case "暴露状态": return "Exposed";
            case "坐骑状态": return "Mounted";

            default: throw new ApplicationException();
        }
    }
    return string.Join(", ", output);
}

string ConvertStat(string chinese)
{
    if (UseReadInt)
    {
        if (byte.TryParse(chinese, out var number))
        {
            if (number > 115)
                throw new ApplicationException();
            return chinese;
        }
    }

    switch (chinese)
    {
        case "未知属性": return "Unknown";
        case "最小防御": return "MinDef";
        case "最大防御": return "MaxDef";
        case "最小魔防": return "MinMCDef";
        case "最大魔防": return "MaxMCDef";
        case "最小攻击": return "MinDC";
        case "最大攻击": return "MaxDC";
        case "最小魔法": return "MinMC";
        case "最大魔法": return "MaxMC";
        case "最小道术": return "MinSC";
        case "最大道术": return "MaxSC";
        case "最小刺术": return "MinNC";
        case "最大刺术": return "MaxNC";
        case "最小弓术": return "MinBC";
        case "最大弓术": return "MaxBC";
        case "最大体力": return "MaxHP";
        case "最大魔力": return "MaxMP";
        case "行走速度": return "WalkSpeed";
        case "奔跑速度": return "RunSpeed";
        case "物理准确": return "PhysicalAccuracy";
        case "物理敏捷": return "PhysicalAgility";
        case "魔法闪避": return "MagicEvade";
        case "暴击概率": return "CriticalHitRate";
        case "暴击伤害": return "CriticalDamage";
        case "减伤害": return "DamageReduction";
        case "药品回血": return "HPRatePercent";
        case "药品回魔": return "MPRatePercent";
        case "幸运等级": return "Luck";
        case "伤害加成": return "DamageBonus";
        case "攻击速度": return "AttackSpeed";
        case "体力恢复": return "HealthRecovery";
        case "魔力恢复": return "ManaRecovery";
        case "中毒躲避": return "PoisonEvade";
        case "技能标志": return "SkillSign";
        case "最小神圣伤害" or "最小圣伤": return "MinHC";
        case "最大神圣伤害" or "最大圣伤": return "MaxHC";
        
        default:
            return chinese; // unstranslated into server
            /*case "怪物伤害":
            case "怪物闪避":
            case "中毒躲避":
                return chinese; // unstranslated into server
            default:
                throw new ApplicationException();*/
    }
}

Dictionary<string, object> ConvertStats(IEnumerable<JProperty> statObj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in statObj)
    {
        output[ConvertStat(prop.Name)] = prop.Value.Value<int>();
    }

    return output;
}

Dictionary<string, object> ConvertInscriptionStat(JObject statObj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in statObj)
    {
        switch (prop.Key)
        {
            case "属性":
                output["Stat"] = ConvertStat(prop.Value.Value<string>());
                break;
            case "零级":
                output["Level0"] = prop.Value.Value<int>();
                break;
            case "一级":
                output["Level1"] = prop.Value.Value<int>();
                break;
            case "二级":
                output["Level2"] = prop.Value.Value<int>();
                break;
            case "三级":
                output["Level3"] = prop.Value.Value<int>();
                break;
        }
    }

    return output;
}

string ConvertBuffActionType(string chinese)
{
    if (UseReadInt)
    {
        switch (chinese)
        {
            case "0" or "1": return chinese;
            default: throw new ApplicationException();
        }
    }

    switch (chinese)
    {
        case "增益类型": return "Gain";
        case "减益类型": return "Debuff";
        default: throw new ApplicationException();
    }
}

string ConvertBuffStackType(string chinese)
{
    if (UseReadInt)
    {
        if (byte.TryParse(chinese, out var number))
        {
            if (number > 3)
                throw new ApplicationException();
            return chinese;
        }
    }

    switch (chinese)
    {
        case "禁止叠加": return "Disabled";
        case "同类替换": return "Substitute";
        case "同类叠加": return "StackStat";
        case "同类延时": return "StackDuration";
        default: throw new ApplicationException();
    }
}

string ConvertBuffEffectType(string chinese)
{
    if (UseReadInt)
    {
        if (uint.TryParse(chinese, out var number))
        {
            if (number > 1024)
                throw new ApplicationException();
            return chinese;
        }
    }

    var values = chinese.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
    var output = new List<string>();
    foreach (var value in values)
    {
        switch (value)
        {
            case "技能标志": output.Add("SkillSign"); break;
            case "状态标志": output.Add("StatusFlag"); break;
            case "造成伤害": output.Add("DealDamage"); break;
            case "属性增减": output.Add("StatIncOrDec"); break;
            case "伤害增减": output.Add("DamageIncOrDec"); break;
            case "创建陷阱": output.Add("CreateTrap"); break;
            case "生命回复": output.Add("HealthRecovery"); break;
            case "诱惑提升": output.Add("TemptationBoost"); break;
            case "坐骑状态" or "坐骑BUFF": output.Add("Mounted"); break;
            case "释放技能": output.Add("UnleashSkills"); break;
            case "添加BUFF": output.Add("AddedBuffs"); break;
            case "获得奖励": output.Add("GiveReward"); break;
            default: throw new ApplicationException();
        }
    }
    return string.Join(", ", output);
}

string ConvertAreaType(string chinese)
{
    switch (chinese)
    {
        case "未知区域": return "Unknown";
        case "复活区域": return "Resurrection";
        case "传送区域": return "Teleportation";
        case "随机区域": return "Random";

        default: return chinese;
    }
}

Dictionary<string, object> ConvertMonsterSpawnInfo(JObject statObj)
{
    var output = new Dictionary<string, object>();

    foreach (var prop in statObj)
    {
        switch (prop.Key)
        {
            case "怪物名字":
                output["MonsterName"] = prop.Value.Value<string>();
                break;
            case "刷新数量":
                output["SpawnCount"] = prop.Value.Value<int>();
                break;
            case "复活间隔":
                output["RevivalInterval"] = prop.Value.Value<int>();
                break;
        }
    }

    return output;
}

DumpBuffs();
DumpSkillsData();
DumpSkillInscriptions();
DumpSkillTraps();
DumpCommonItems();
DumpEquipmentItems();
DumpItemSets();
DumpEquipmentStats();
DumpRareTreasureItems();
DumpGameStores();
DumpMonsters();
DumpMaps();
DumpMapAreas();
DumpMapSpawns();
DumpMapGuards();
DumpMapTeleports();
DumpMounts();
DumpMountBeasts();
DumpGameTitles();
DumpGuards();
DumpNPCDialogs();
DumpItemRandomStats();