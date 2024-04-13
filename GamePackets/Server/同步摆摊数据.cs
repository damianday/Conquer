namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 23, Length = 6, Description = "同步摆摊数据")]
public sealed class 同步摆摊数据 : GamePacket
{
}
