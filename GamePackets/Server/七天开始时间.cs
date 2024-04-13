namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 358, Length = 10, Description = "g2c_seven_days_start_time")]
public sealed class 七天开始时间 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 开始时间;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 参数二;
}
