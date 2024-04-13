namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1007, Length = 6, Description = "帧同步, 请求Ping")]
public sealed class 测试网关网速 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int UserTime;
}
