namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 326, Length = 52, Description = "记录部位回执")]
public sealed class 记录部位回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 记录序号;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 记录部位;

	[FieldAttribute(Position = 4, Length = 48)]
	public byte[] 数据;
}
