namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 372, Length = 10, Description = "同步传奇之力")]
public sealed class 同步传奇之力 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 传奇之力;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 对象编号;
}
