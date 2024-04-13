namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 217, Length = 10, Description = "放入交易金币")]
public sealed class 放入交易金币 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 金币数量;
}
