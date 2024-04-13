namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 23, Length = 6, Description = "帧同步, 请求Ping")]
public sealed class 客户网速测试 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int UserTime;
}
