namespace GameServer.Template;

public sealed class A_01_TriggerObjectBuff : SkillTask
{
    public bool 角色自身添加;
    public ushort 触发Buff编号;
    public ushort 伴生Buff编号;
    public float Buff触发概率;
    public bool ValidateInscriptionSkill;
    public ushort RequiredInscriptionID;
    public bool 同组铭文无效;
    public bool 验证自身Buff;
    public ushort 自身Buff编号;
    public bool 触发成功移除;
    public bool 移除伴生Buff;
    public ushort 移除伴生编号;
    public bool 验证分组Buff;
    public ushort BuffGroupID;
    public bool VerifyTargetBuff;
    public ushort 目标Buff编号;
    public byte 所需Buff层数;
    public bool VerifyTargetType;
    public SpecifyTargetType RequiredTarget;
    public bool GainSkillExp;
    public ushort ExpSkillID;

    public bool 验证效果取反;
    public ushort 增减目标BUFF;
    public int 增减BUFF层数;
}
