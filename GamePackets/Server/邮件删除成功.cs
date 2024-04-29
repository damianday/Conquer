namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 577, Length = 14, Description = "邮件删除成功")]
public sealed class 邮件删除成功 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int MailID;
}
