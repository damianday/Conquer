namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 216, Length = 4, Description = "领取特权礼包")]
public sealed class 领取特权礼包 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 特权类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 礼包位置;
}
