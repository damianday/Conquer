namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 604, Length = 10, Description = "查询排名榜单")]
public sealed class 查询排名榜单 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 榜单类型;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 起始位置;
}
