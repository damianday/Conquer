namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 79, Length = 6, Description = "同步对象魔力")]
public sealed class SyncManaPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CurrentMP;
}
