namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 579, Length = 14, Description = "提取附件成功")]
public sealed class 成功提取附件 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int MailID;
}
