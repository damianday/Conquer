namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 558, Length = 7, Description = "处理入会申请")]
public sealed class 处理入会申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 处理类型;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 对象编号;
}
