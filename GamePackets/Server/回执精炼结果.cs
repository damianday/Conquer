namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 380, Length = 16, Description = "回执精炼结果")]
public sealed class 回执精炼结果 : GamePacket
{
	[FieldAttribute(Position = 10, Length = 2)]
	public ushort 结果值一;

	[FieldAttribute(Position = 12, Length = 2)]
	public ushort 结果值二;

	[FieldAttribute(Position = 14, Length = 2)]
	public ushort 结果值三;
}
