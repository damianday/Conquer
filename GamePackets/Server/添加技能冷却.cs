namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 109, Length = 10, Description = "添加技能冷却")]
public sealed class 添加技能冷却 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 冷却编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 冷却时间;
}
