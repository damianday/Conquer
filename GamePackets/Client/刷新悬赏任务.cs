namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 128, Length = 6, Description = "刷新悬赏任务")]
public sealed class 刷新悬赏任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 对话编号;
}
