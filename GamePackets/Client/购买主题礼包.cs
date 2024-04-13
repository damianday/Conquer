namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 278, Length = 22, Description = "购买主题礼包")]
public sealed class 购买主题礼包 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 日期序号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 物品A_ID;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 物品B_ID;

	[FieldAttribute(Position = 14, Length = 4)]
	public int 物品C_ID;

	[FieldAttribute(Position = 18, Length = 4)]
	public int 物品D_ID;
}
