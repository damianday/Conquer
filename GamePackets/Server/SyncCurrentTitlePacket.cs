namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 83, Length = 7, Description = "玩家装配称号", Broadcast = true)]
public sealed class SyncCurrentTitlePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte TitleID;
}
