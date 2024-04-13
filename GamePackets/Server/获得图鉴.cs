namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 222, Length = 10, Description = "获得图鉴")]
public sealed class 获得图鉴 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 图鉴类型;

	[FieldAttribute(Position = 2, Length = 4)]
	public int 图鉴编号;
}
