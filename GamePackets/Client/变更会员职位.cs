namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 565, Length = 7, Description = "变更会员职位")]
public sealed class 变更会员职位 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 对象职位;
}
