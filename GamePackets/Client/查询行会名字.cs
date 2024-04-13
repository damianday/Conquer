namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 552, Length = 10, Description = "查询行会名字")]
public sealed class 查询行会名字 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 状态编号;
}
