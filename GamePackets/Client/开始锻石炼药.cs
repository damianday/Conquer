namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 270, Length = 16, Description = "开始锻石炼药")]
public sealed class 开始锻石炼药 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 模版编号;

	[FieldAttribute(Position = 6, Length = 1)]
	public byte 基础材料容器;

	[FieldAttribute(Position = 7, Length = 1)]
	public byte 基础材料位置;

	[FieldAttribute(Position = 8, Length = 1)]
	public byte 额外材料容器;

	[FieldAttribute(Position = 9, Length = 1)]
	public byte 额外材料位置;

	[FieldAttribute(Position = 10, Length = 1)]
	public byte 额外材料二容器;

	[FieldAttribute(Position = 11, Length = 1)]
	public byte 额外材料二位置;

	[FieldAttribute(Position = 12, Length = 4)]
	public int 未知参数;
}
