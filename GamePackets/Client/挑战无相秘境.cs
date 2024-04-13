namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 262, Length = 4, Description = "挑战无相秘境")]
public sealed class 挑战无相秘境 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 背包位置;
}
