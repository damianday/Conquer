namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1, Length = 15, Description = "GatewayQueryResponsePacket")]
public sealed class GatewayQueryResponsePacket : GamePacket
{
}
