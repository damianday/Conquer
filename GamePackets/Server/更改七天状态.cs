namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 357, Length = 12, Description = "g2c_seven_days_task_state")]
public sealed class 更改七天状态 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 状态标志;

	[FieldAttribute(Position = 4, Length = 4)]
	public int 任务编号;

	[FieldAttribute(Position = 8, Length = 4)]
	public int 活动积分;
}
