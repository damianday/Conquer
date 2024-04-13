namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 182, Length = 6, Description = "领取日程奖励")]
public sealed class 领取日程奖励 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 奖励进度;
}
