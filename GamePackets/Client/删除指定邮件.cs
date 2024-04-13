namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 550, Length = 14, Description = "删除指定邮件")]
public sealed class 删除指定邮件 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int 邮件编号;
}
