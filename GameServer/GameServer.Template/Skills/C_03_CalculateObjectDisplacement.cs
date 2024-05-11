namespace GameServer.Template;

public sealed class C_03_CalculateObjectDisplacement : SkillTask
{
    public bool 角色自身位移;
    public bool 允许超出锚点;
    public bool 锚点反向位移;
    public bool DisplacementIncreaseExp;
    public bool 多段位移通知;
    public bool 能否穿越障碍;
    public ushort 自身位移耗时;
    public ushort 自身硬直时间;
    public byte[] 自身位移次数;
    public byte[] 自身位移距离;
    public ushort 成功Buff编号;
    public float 成功Buff概率;
    public ushort 失败Buff编号;
    public float 失败Buff概率;
    public bool 推动目标位移;
    public bool 推动增加经验;
    public float 推动目标概率;
    public SpecifyTargetType 推动目标类型;
    public byte 连续推动数量;
    public ushort 目标位移耗时;
    public byte[] 目标位移距离;
    public ushort 目标硬直时间;
    public ushort 目标位移编号;
    public float 位移Buff概率;
    public ushort 目标附加编号;
    public SpecifyTargetType 限定附加类型;
    public float 附加Buff概率;
    public int 限定锚点距离;

    public ushort 成功减少节点;
    public int 每格减少时间;
    public int 失败触发节点;
    public int 角色位移方式;
    public bool 互换目标坐标;
    public int 互换最大距离;
    public bool 反向推动目标;
}
