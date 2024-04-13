namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 618, Length = 9, Description = "上架平台商品")]
public sealed class 上架平台商品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 背包位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 时间类型;

    [FieldAttribute(Position = 5, Length = 4)]
    public int 上架价格;
}
