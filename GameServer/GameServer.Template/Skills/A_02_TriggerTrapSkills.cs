namespace GameServer.Template;

public sealed class A_02_TriggerTrapSkills : SkillTask
{
    public SkillTriggerMethod SkillTriggerMethod;
    public string TriggerTrapSkills;
    public ObjectSize NumberTrapsTriggered;
    public int TrapSpacing;
    public bool GainSkillExp;
    public ushort ExpSkillID;

    //public CustomRange; // 个性技能范围
    public bool EnableCustomRange;
    public bool 计算锚点自身;
    public bool 出生限定方向;
    public GameDirection TrapBirthDirection;
    public int 陷阱触发概率;
    public bool 追踪命中目标;
}
