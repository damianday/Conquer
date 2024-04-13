namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 620, Length = 10, Description = "购买平台物品")]
public sealed class 购买平台物品 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 8)]
	public uint 订单编号;
}
