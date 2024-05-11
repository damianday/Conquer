using System.Collections.Generic;

namespace GameServer.Template;

public sealed class C_01_CalculateHitTarget : SkillTask
{
    public bool ClearHitList;
    public bool PassThroughWall;
    public bool 技能能否招架;
    public SkillLockType SkillLockMode;
    public SkillEvasionType SkillEvasion;
    public SkillHitFeedback SkillHitFeedback;
    public ObjectSize SkillRangeType;
    public bool 放空结束技能;
    public bool SendInterruptNotification;
    public bool 补发释放通知;
    public bool SendSkillHitNotification;
    public bool SendSkillExpansionNotification;
    public bool CalculateFlightTime;
    public int SingleCellFlightTime;
    public int HitsLimit;
    public GameObjectType LimitTargetType;
    public GameObjectRelationship LimitTargetRelationship;
    public SpecifyTargetType LimitSpecificType;
    public SpecifyTargetType AttackSpeedIncreaseType;
    public int AttackSpeedIncrease;
    public bool TriggerPassiveSkills;
    public float TriggerPassiveSkillsProbability;
    public bool GainSkillExp;
    public ushort ExpSkillID;
    public bool ClearTargetStatus;
    public HashSet<ushort> ClearStatusList;

    //public CustomRange // 个性技能范围;
    public bool EnableCustomRange;
    public bool CalculateObjectDirection;
    public bool 计算锚点自身;
    public int 宠物模板编号;
}
