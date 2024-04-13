namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 78, Length = 14, Description = "同步对象体力", Broadcast = true)]
public sealed class SyncObjectHP : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int CurrentHP;

    [FieldAttribute(Position = 10, Length = 4)]
    public int MaxHP;
}
