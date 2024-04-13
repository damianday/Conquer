namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 53, Length = 4, Description = "商店修理单件")]
public sealed class 商店修理单件 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;
}
