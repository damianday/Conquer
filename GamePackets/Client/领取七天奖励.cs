namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 253, Length = 7, Description = "领取七天奖励")]
public sealed class 领取七天奖励 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 未知参数;

	[FieldAttribute(Position = 3, Length = 4)]
	public int 领取编号;
}
