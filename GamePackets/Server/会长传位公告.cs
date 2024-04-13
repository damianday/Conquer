namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 603, Length = 10, Description = "会长传位公告")]
public sealed class 会长传位公告 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 当前编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 传位编号;
}
