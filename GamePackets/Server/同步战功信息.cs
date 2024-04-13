namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 375, Length = 34, Description = "同步战功信息")]
public sealed class 同步战功信息 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 开始时间;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 结束时间;

	[FieldAttribute(Position = 10, Length = 2)]
	public ushort 战功进度;

	[FieldAttribute(Position = 12, Length = 2)]
	public ushort 购买战功;

	[FieldAttribute(Position = 14, Length = 2)]
	public ushort 战功奖励;

	[FieldAttribute(Position = 16, Length = 2)]
	public ushort 军机奖励;

	[FieldAttribute(Position = 18, Length = 4)]
	public int 战功状态;

	[FieldAttribute(Position = 22, Length = 2)]
	public ushort 购买次数;

	[FieldAttribute(Position = 24, Length = 2)]
	public ushort 未知参数二;

	[FieldAttribute(Position = 26, Length = 4)]
	public int 开始时间二;

	[FieldAttribute(Position = 30, Length = 4)]
	public int 未知参数三;
}
