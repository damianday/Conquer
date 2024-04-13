namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 65, Length = 6, Description = "请求商店数据")]
public sealed class 请求商店数据 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 版本编号;
}
