namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 175, Length = 6, Description = "成就进度改变")]
public sealed class 杀怪成就变更 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 成就序号;

	[FieldAttribute(Position = 4, Length = 2)]
	public ushort 成就进度;
}
