namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 257, Length = 10, Description = "注入天赋之力")]
public sealed class 注入天赋之力 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 天赋位置;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 未知参数;
}
