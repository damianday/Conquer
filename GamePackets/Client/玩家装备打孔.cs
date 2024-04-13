namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 95, Length = 4, Description = "玩家装备打孔")]
public sealed class 玩家装备打孔 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 物品位置;
}
