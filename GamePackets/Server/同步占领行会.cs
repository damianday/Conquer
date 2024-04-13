namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 663, Length = 10, Description = "同步占领行会")]
public sealed class 同步占领行会 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int OldGuildID;

	[FieldAttribute(Position = 6, Length = 4)]
	public int NewGuildID;
}