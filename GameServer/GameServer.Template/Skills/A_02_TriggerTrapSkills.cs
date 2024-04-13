namespace GameServer.Template;

public sealed class A_02_TriggerTrapSkills : SkillTask
{
    public SkillTriggerMethod 技能触发方式;
    public string TriggerTrapSkills;
    public ObjectSize NumberTrapsTriggered;
    public int TrapSpacing;
    public bool GainSkillExp;
    public ushort ExpSkillID;
}
