namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 80, Length = 10, Description = "同步对象惩罚", Broadcast = true)]
public sealed class SyncPKPointPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int PKPoint;
}
