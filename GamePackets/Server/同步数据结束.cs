namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 33, Length = 6, Description = "同步游戏数据结束")]
public sealed class 同步数据结束 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;
}
