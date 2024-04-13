namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 367, Length = 6, Description = "锻石炼药回执")]
public sealed class 锻石炼药回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 回执编号;
}
