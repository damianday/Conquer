namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 96, Length = 12, Description = "玩家装备雕色")]
public sealed class 玩家装备雕色 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 物品位置;

	[FieldAttribute(Position = 4, Length = 4)]
	public int 孔洞位置;

	[FieldAttribute(Position = 8, Length = 4)]
	public int 孔洞颜色;
}
