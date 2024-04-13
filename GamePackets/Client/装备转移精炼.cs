namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 284, Length = 6, Description = "装备转移精炼")]
public sealed class 装备转移精炼 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 背包位置;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 材料类型;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte 材料位置;
}
