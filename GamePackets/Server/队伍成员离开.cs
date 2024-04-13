namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 519, Length = 10, Description = "队伍成员离开")]
public sealed class 队伍成员离开 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 队伍编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 对象编号;
}
