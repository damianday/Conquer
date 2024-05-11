namespace GameServer.Template;

public sealed class B_01_SkillReleaseNotification : SkillTask
{
    public bool SendReleaseNotification;
    public bool 移除技能标记;
    public bool UpdateCharacterDirection;
    public int SelfCooldown;
    public bool Buff增加冷却;
    public ushort 增加冷却Buff;
    public int CooldownIncreaseTime;
    public int GroupCooldownTime;
    public int 角色忙绿时间;
    public bool GainSkillExp;
    public ushort ExpSkillID;

    public bool Buff增加层数;
    public ushort 增加层数Buff;
    public int 增加Buff层数;
    public string SendMapAnnouncement;
}
