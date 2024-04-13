namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 238, Length = 8, Description = "回应组队请求")]
public sealed class 回应组队请求 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 组队方式;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte 回应方式;
}
