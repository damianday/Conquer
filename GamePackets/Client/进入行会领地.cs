namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 172, Length = 7, Description = "进入行会领地")]
public sealed class 进入行会领地 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 地图类型;

	[FieldAttribute(Position = 3, Length = 4)]
	public int 行会编号;
}
