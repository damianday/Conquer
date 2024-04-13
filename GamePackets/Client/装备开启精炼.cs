namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 281, Length = 4, Description = "装备开启精炼")]
public sealed class 装备开启精炼 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 背包位置;
}
