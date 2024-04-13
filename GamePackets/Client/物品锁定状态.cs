namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 97, Length = 5, Description = "物品锁定状态")]
public sealed class 物品锁定状态 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 物品位置;

	[FieldAttribute(Position = 4, Length = 1)]
	public bool 锁定状态;
}
