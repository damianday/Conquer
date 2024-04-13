namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 256, Length = 6, Description = "UnknownC255")]
public sealed class UnknownC255 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int U1;
}
