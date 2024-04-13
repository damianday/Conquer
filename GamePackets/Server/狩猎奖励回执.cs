namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 301, Length = 14, Description = "g2c_sync_advanced_exercise")]
public sealed class 狩猎奖励回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 回执结果;

	// TODO: Check this
	/*[FieldAttribute(Position = 2, Length = 2)]
	public ushort 未知标志;

	[FieldAttribute(Position = 4, Length = 2)]
	public ushort 未知参数;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 狩猎编号;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 剩余秒数;*/
}
