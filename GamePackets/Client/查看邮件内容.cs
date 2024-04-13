namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 549, Length = 14, Description = "查看邮件内容")]
public sealed class 查看邮件内容 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int 邮件编号;
}
