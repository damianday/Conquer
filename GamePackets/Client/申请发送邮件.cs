namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 547, Length = 0, Description = "申请发送邮件")]
public sealed class 申请发送邮件 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] 字节数据;
}
