namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 549, Length = 7, Description = "同步师徒等级")]
public sealed class 同步师徒等级 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte CurrentLevel;
}
