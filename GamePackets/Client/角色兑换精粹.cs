namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 91, Length = 4, Description = "角色兑换精粹")]
public sealed class 角色兑换精粹 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 物品位置;
}
