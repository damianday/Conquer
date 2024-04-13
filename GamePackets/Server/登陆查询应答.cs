namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1010, Length = 14, Description = "同步网关ping")]
public sealed class 登陆查询应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 当前时间;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 未知参数1 = 24174;

    [FieldAttribute(Position = 10, Length = 4)]
    public int 未知参数2 = 7;
}
