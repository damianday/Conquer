namespace GameServer.Template;

public sealed class B_03_FrontSwingEndNotification : SkillTask
{
    public bool SendEndNotification;
    public bool CalculateAttackSpeedReduction;
    public int AttackTimeReduction;
    public int WalkTimeReduction;
    public int RunTimeReduction;
    public bool DisarmTrapSkills;

    public ushort 发送特殊标记;
}
