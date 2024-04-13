namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 110, Length = 7, Description = "同步对战模式")]
public sealed class SyncAttackModePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte Mode;
}
