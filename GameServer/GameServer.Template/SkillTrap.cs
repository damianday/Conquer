using System.Collections.Generic;
using System.IO;

namespace GameServer.Template;

public sealed class SkillTrap
{
    public static Dictionary<string, SkillTrap> DataSheet;

    public string Name;
    public ushort ID;
    public ushort GroupID;
    //public ushort TrapGroupID;
    public ObjectSize Size;
    public ushort BindingLevel;
    public bool AllowStacking;
    public int Duration;
    public bool ExtendedDuration;
    public bool SkillLevelDelay;
    public int ExtendedTimePerLevel;
    public bool PlayerStatDelay;
    public Stat BoundPlayerStat;
    public float StatDelayFactor;
    public bool HasSpecificInscriptionDelay;
    public InscriptionSkill BindInscriptionSkill;
    public int SpecificInscriptionSkills;
    public int InscriptionExtendedTime;
    public bool CanMove;
    public ushort MoveSpeed;
    public byte LimitMoveSteps;
    public bool MoveInCurrentDirection;
    public bool ActivelyPursueEnemy;
    public byte PursuitRange;
    public string PassiveTriggerSkill;
    public bool RetriggeringIsProhibited;
    public SpecifyTargetType PassiveTargetType;
    public GameObjectType PassiveObjectType;
    public GameObjectRelationship PassiveType;
    public string ActivelyTriggerSkills;
    public ushort ActiveTriggerInterval;
    public ushort ActiveTriggerDelay;

    public static void LoadData()
    {
        DataSheet = new Dictionary<string, SkillTrap>();

        var path = Settings.Default.GameDataPath + "\\System\\Skills\\Traps\\";
        if (!Directory.Exists(path))
            return;

        var array = Serializer.Deserialize<SkillTrap>(path);
        foreach (var obj in array)
            DataSheet.Add(obj.Name, obj);
    }
}
