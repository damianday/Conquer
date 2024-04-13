namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 215, Length = 11, Description = "交易状态改变")]
public sealed class 交易状态改变 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 交易状态;

    [FieldAttribute(Position = 7, Length = 4)]
    public int 对象等级;
}
