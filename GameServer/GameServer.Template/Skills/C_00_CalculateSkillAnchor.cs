namespace GameServer.Template;

public sealed class C_00_CalculateSkillAnchor : SkillTask
{
    public bool CalculateCurrentPosition;
    public bool CalculateCurrentDirection;
    public int MinDistance;
    public int MaxDistance;

    public bool 计算BUFF目标;
    public bool 验证BUFF来源;
    public ushort 目标BUFF编号;
    public ushort 搜索目标范围;
    public bool 目标前方位置;
}
