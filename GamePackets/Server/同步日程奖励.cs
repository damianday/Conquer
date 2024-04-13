namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 281, Length = 4, Description = "g2c_activity_reward")]
public sealed class 同步日程奖励 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 奖励挡位;
}
