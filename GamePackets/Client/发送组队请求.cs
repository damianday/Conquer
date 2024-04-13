namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 516, Length = 6, Description = "发送组队请求")]
public sealed class 发送组队请求 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
