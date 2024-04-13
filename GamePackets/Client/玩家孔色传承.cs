namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 99, Length = 6, Description = "玩家孔色传承")]
public sealed class 玩家孔色传承 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 来源未知;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 来源位置;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 传承未知;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte 传承位置;
}
