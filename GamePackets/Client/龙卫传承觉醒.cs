namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 220, Length = 10, Description = "龙卫传承觉醒")]
public sealed class 龙卫传承觉醒 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 属性位置;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 当前阶段;
}
