namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1, Length = 15, Description = "网关查询应答")]
public sealed class 网关查询应答 : GamePacket
{
}
