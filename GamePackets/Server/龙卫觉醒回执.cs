namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 324, Length = 100, Description = "龙卫觉醒回执")]
public sealed class 龙卫觉醒回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 特效编号;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 属性位置;

	[FieldAttribute(Position = 28, Length = 72)]
	public byte[] 数据;
}
