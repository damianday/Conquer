namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 50, Length = 10, Description = "对象转动", Broadcast = true)]
public sealed class SyncObjectDirectionPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort ActionTime;

    [FieldAttribute(Position = 8, Length = 2)]
    public ushort Direction;
}
