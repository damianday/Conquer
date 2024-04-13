namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 62, Length = 7, Description = "对象离开视野")]
public sealed class ObjectDisappearPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte Mode;
}
