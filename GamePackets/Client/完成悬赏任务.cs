namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 129, Length = 18, Description = "完成悬赏任务")]
public sealed class 完成悬赏任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 物品编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 物品容器;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 物品位置;

	[FieldAttribute(Position = 14, Length = 4)]
	public int 任务编号;
}
