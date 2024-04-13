namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 166, Length = 6, Description = "完成任务回执")]
public sealed class 完成任务回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 任务编号;
}
