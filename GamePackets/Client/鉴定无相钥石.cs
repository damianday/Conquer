namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 263, Length = 4, Description = "鉴定无相钥石")]
public sealed class 鉴定无相钥石 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 背包位置;
}
