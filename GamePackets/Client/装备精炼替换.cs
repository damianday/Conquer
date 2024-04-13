namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 283, Length = 4, Description = "装备精炼替换")]
public sealed class 装备精炼替换 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 背包类型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 背包位置;
}
