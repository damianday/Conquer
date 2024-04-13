namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 176, Length = 5, Description = "杀怪成就回执")]
public sealed class 杀怪成就回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 成就序号;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 进度编号;
}
