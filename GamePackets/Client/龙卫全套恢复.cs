namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 239, Length = 4, Description = "龙卫全套恢复")]
public sealed class 龙卫全套恢复 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 恢复模式;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 记录序号;
}
