namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 617, Length = 6, Description = "仓库移动应答")]
public sealed class 仓库移动应答 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 来源页面;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 来源位置;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 目标页面;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte 目标位置;
}