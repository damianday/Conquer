namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 287, Length = 3, Description = "请求悬赏任务")]
public sealed class 请求悬赏任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 任务类型;
}
