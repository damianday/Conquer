namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 275, Length = 6, Description = "领取战功奖励")]
public sealed class 领取战功奖励 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 领取类型;
}
