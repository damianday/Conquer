namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 194, Length = 32, Description = "同步队友信息")]
public sealed class 同步队友信息 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 对象编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 对象等级;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 最大体力;

	[FieldAttribute(Position = 14, Length = 4)]
	public int 最大魔力;

	[FieldAttribute(Position = 18, Length = 4)]
	public int 当前体力;

	[FieldAttribute(Position = 22, Length = 4)]
	public int 当前魔力;

	[FieldAttribute(Position = 26, Length = 2)]
	public ushort 横向坐标;

	[FieldAttribute(Position = 28, Length = 2)]
	public ushort 纵向坐标;

	[FieldAttribute(Position = 30, Length = 2)]
	public ushort 坐标高度;
}
