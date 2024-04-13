using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public sealed class GameSkill
{
    public static Dictionary<string, GameSkill> DataSheet;

    public string SkillName;
    public GameObjectRace Race;
    public GameSkillType SkillType;
    public ushort OwnSkillID;
    public byte ID;
    public byte GroupID;
    public ushort BindingLevelID;
    public bool NeedMoveForward;
    public byte MaxDistance;
    public bool CalculateLuckyProbability;
    public float CalculateTriggerProbability;
    public Stat StatBoostProbability;
    public float StatBoostFactor;
    public bool CheckBusyGreen;
    public bool CheckStunStatus;
    public bool CheckOccupationalWeapons;
    public bool CheckPassiveTags;
    public bool CheckSkillMarks;
    public bool CheckSkillCount;
    public ushort SkillTagID;
    public int[] NeedConsumeMagic; //Requires mana consumption
    public HashSet<int> NeedConsumeItems;
    public int NeedConsumeItemsQuantity;
    public int GearDeductionPoints;
    public ushort ValidateLearnedSkills;
    public byte VerficationSkillInscription;
    public ushort VerifyPlayerBuff;
    public int PlayerBuffStackCount = 1;
    public SpecifyTargetType VerifyTargetType;
    public ushort VerifyTargetBuff;
    public int TargetBuffStackCount = 1;
    public int TargetClosestDistance;
    public int TargetFurthestDistance;
    public bool AutomaticAssembly;
    public ushort 友方技能编号;
    public bool 增加技能经验;
    public SortedDictionary<int, SkillTask> Nodes = new SortedDictionary<int, SkillTask>();

    public static void LoadData()
    {
        DataSheet = new Dictionary<string, GameSkill>();

        var path = Config.GameDataPath + "\\System\\Skills\\Skills\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<GameSkill>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.SkillName, obj);
    }
}
