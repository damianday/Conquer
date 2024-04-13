namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 55, Length = 7, Description = "对象死亡", Broadcast = true)]
public sealed class ObjectDiePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;
}
