namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 647, Length = 11, Description = "同步会员信息")]
public sealed class SyncMemberInfoPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 对象信息;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte CurrentLevel;
}
