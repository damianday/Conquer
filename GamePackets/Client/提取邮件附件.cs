namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 551, Length = 14, Description = "提取邮件附件")]
public sealed class 提取邮件附件 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int 邮件编号;
}
