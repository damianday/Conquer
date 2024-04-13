namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 323, Length = 0, Description = "同步龙卫信息")]
public sealed class 同步龙卫信息 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 1)]
	public byte 未知;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte 格子数量;

	[FieldAttribute(Position = 6, Length = 1)]
	public byte 记录数量;

	[FieldAttribute(Position = 7, Length = 1)]
	public byte 可用记录;

	[FieldAttribute(Position = 8, Length = 0)]
	public byte[] 描述信息;
}
