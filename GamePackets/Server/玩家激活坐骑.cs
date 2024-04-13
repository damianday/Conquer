namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 144, Length = 7, Description = "g2c_activate_mount")]
public sealed class 玩家激活坐骑 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 对象编号;

	[FieldAttribute(Position = 6, Length = 1)]
	public byte 坐骑编号;
}
