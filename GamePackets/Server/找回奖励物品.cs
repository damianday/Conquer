namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 400, Length = 14, Description = "找回奖励物品")]
public sealed class 找回奖励物品 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 找回结果;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 日程编号;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 剩余次数;
}
