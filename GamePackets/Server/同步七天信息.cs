namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 355, Length = 679, Description = "g2c_seven_days_info")]
public sealed class 同步七天信息 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 677)]
	public byte[] 字节描述;
}
