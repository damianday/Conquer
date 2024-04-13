namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 214, Length = 4, Description = "玩家失去称号")]
public sealed class RemoveTitlePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte TitleID;
}
