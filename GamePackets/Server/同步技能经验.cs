namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 126, Length = 6, Description = "同步技能经验")]
public sealed class 同步技能经验 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort 技能编号;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort 当前经验;
}
