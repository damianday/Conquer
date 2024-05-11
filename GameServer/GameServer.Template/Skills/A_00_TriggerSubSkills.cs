namespace GameServer.Template;

public sealed class A_00_TriggerSubSkills : SkillTask
{
    public SkillTriggerMethod SkillTriggerMethod;
    public string TriggerSkillName;
    public string BackhandSkillName;
    public bool CalculateTriggerProbability;
    public bool CalculateLuckyProbability;
    public float SkillTriggerProbability;
    public ushort IncreaseProbabilityBuffID;
    public float BuffIncreaseFactor;
    public bool 验证自身Buff;
    public ushort 自身Buff编号;
    public bool 触发成功移除;
    public bool ValidateInscriptionSkill;
    public ushort RequiredInscriptionID;
    public bool 同组铭文无效;

    public int TargetDistanceMin;
    public bool 检测技能等级;
    public int RequiredSkillLevel;
    public ushort 检测BUFF层数;
    public bool 验证目标Buff;
    public ushort 目标Buff编号;
    public bool 触发成功结束;
    public bool 不发结束通知;
    public int RequiredWeaponID;
    public bool DetectRequiredWeapon;
    public bool 检测目标数量;
    public int 限定目标数量;
    public bool 增加动作编号;
    public int 目标距离大于;
}
