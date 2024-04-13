using System.Collections.Generic;

namespace GameServer.Template;

public sealed class C_01_CalculateHitTarget : SkillTask
{
    public bool 清空命中列表;
    public bool 技能能否穿墙;
    public bool 技能能否招架;
    public SkillLockType 技能锁定方式;
    public SkillEvasionType SkillEvasion;
    public SkillHitFeedback SkillHitFeedback;
    public ObjectSize 技能范围类型;
    public bool 放空结束技能;
    public bool 发送中断通知;
    public bool 补发释放通知;
    public bool 技能命中通知;
    public bool 技能扩展通知;
    public bool 计算飞行耗时;
    public int 单格飞行耗时;
    public int HitsLimit;
    public GameObjectType LimitTargetType;
    public GameObjectRelationship LimitTargetRelationship;
    public SpecifyTargetType LimitSpecificType;
    public SpecifyTargetType 攻速提升类型;
    public int 攻速提升幅度;
    public bool 触发被动技能;
    public float 触发被动概率;
    public bool GainSkillExp;
    public ushort ExpSkillID;
    public bool 清除目标状态;
    public HashSet<ushort> 清除状态列表;
}
