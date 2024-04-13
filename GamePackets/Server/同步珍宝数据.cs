namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 658, Length = 0, Description = "请求珍宝数据")]
public sealed class 同步珍宝数据 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int 版本编号;

    [FieldAttribute(Position = 8, Length = 4)]
    public int 商品数量;

    [FieldAttribute(Position = 12, Length = 0)]
    public byte[] 商店数据;
}
