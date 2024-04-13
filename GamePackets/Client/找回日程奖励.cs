namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 235, Length = 10, Description = "找回日程奖励")]
public sealed class 找回日程奖励 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 日程编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 找回次数;
}
