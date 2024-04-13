namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 303, Length = 6, Description = "g2c_advanced_exercise_ack")]
public sealed class 开始狩猎回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 回执结果;
}
