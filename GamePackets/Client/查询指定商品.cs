namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 616, Length = 6, Description = "查询指定商品")]
public sealed class 查询指定商品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 物品编号;
}
