namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 361, Length = 18, Description = "天赋之力回执")]
public sealed class 天赋之力回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 天赋位置;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 天赋等级;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 天赋进度;

	[FieldAttribute(Position = 14, Length = 4)]
	public int 激活刻印;
}
