namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 578, Length = 6, Description = "未读邮件提醒")]
public sealed class SyncNewMailPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int MessageCount;
}