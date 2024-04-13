namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 590, Length = 6, Description = "处理入会申请")]
public sealed class 入会申请应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
