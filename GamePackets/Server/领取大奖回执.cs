namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 356, Length = 15, Description = "g2c_seven_days_next_score_award")]
public sealed class 领取大奖回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public bool 是否失败;

	[FieldAttribute(Position = 3, Length = 4)]
	public int 下次奖励;

	[FieldAttribute(Position = 11, Length = 4)]
	public int 奖励数量;
}
