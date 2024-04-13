namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 130, Length = 5, Description = "领取杀怪成就")]
public sealed class 领取杀怪成就 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 成就编号;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 进度编号;
}
