namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 619, Length = 10, Description = "下架寄售物品")]
public sealed class 下架寄售物品 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 8)]
	public uint 订单编号;
}
