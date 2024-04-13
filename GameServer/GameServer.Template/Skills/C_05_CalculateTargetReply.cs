namespace GameServer.Template;

public sealed class C_05_CalculateTargetReply : SkillTask
{
    public int[] 体力回复次数;
    public float[] 道术叠加次数;
    public byte[] PhysicalRecoveryBase;
    public float[] 道术叠加基数;
    public int[] 立即回复基数;
    public float[] 立即回复系数;
    public bool GainSkillExp;
    public ushort ExpSkillID;
}
