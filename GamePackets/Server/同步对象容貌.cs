namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 309, Length = 10, Description = "同步对象容貌")]
public sealed class 同步对象容貌 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 对象编号;

	[FieldAttribute(Position = 6, Length = 1)]
	public byte 对象发型;

	[FieldAttribute(Position = 7, Length = 1)]
	public byte 对象发色;

	[FieldAttribute(Position = 8, Length = 1)]
	public byte 对象脸型;

	[FieldAttribute(Position = 9, Length = 1)]
	public byte 未知参数;
}
