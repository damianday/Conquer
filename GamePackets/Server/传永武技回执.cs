namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 365, Length = 6, Description = "传永武技回执")]
public sealed class 传永武技回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 回执编号;
}
