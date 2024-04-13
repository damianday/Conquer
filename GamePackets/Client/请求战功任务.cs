namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 274, Length = 6, Description = "请求战功任务")]
public sealed class 请求战功任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 任务类型;
}
