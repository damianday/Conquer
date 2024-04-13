namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 221, Length = 11, Description = "龙卫传承重塑")]
public sealed class 龙卫传承重塑 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 属性位置;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 模式;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 附加;
}
