namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 599, Length = 6, Description = "请求珍宝数据")]
public sealed class 请求珍宝数据 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 数据版本;
}
