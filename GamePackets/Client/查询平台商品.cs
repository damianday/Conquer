namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 615, Length = 6, Description = "查询平台商品")]
public sealed class 查询平台商品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 过滤筛选;
}
