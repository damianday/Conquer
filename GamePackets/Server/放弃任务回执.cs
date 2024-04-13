namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 165, Length = 10, Description = "放弃任务回执")]
public sealed class 放弃任务回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 任务编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 回执结果;
}
