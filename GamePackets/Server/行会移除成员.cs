namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 594, Length = 6, Description = "行会移除成员")]
public sealed class 行会移除成员 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
