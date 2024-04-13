namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 659, Length = 10, Description = "购买珍宝商品")]
public sealed class 购入珍宝商品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 物品编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 购买数量;
}
