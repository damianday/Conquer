namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 51, Length = 2, Description = "同步回购列表")]
public sealed class 查询回购列表 : GamePacket
{
}
