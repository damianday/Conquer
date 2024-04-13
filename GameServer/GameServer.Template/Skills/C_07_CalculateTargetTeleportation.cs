namespace GameServer.Template;

public sealed class C_07_CalculateTargetTeleportation : SkillTask
{
    public float[] 每级成功概率;
    public ushort 瞬移失败提示;
    public ushort 失败添加Buff;
    public bool GainSkillExp;
    public ushort ExpSkillID;
}
