namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 662, Length = 191, Description = "查询排行榜单")]
public sealed class 查询排行榜单 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 189)]
    public byte[] 字节数据;
}
