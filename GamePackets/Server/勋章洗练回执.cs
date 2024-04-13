namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 262, Length = 10, Description = "勋章洗练回执")]
public sealed class 勋章洗练回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 属性一;

	[FieldAttribute(Position = 4, Length = 2)]
	public ushort 属性二;

	[FieldAttribute(Position = 6, Length = 2)]
	public ushort 属性三;

	[FieldAttribute(Position = 8, Length = 2)]
	public ushort 属性四;
}
