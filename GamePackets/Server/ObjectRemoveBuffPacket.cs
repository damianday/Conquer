namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 117, Length = 10, Description = "对象移除状态", Broadcast = true)]
public sealed class ObjectRemoveBuffPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int BuffID;
}
