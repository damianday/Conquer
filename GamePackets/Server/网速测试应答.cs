namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 45, Length = 6, Description = "同步游戏ping")]
public sealed class 网速测试应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 当前时间;
}
