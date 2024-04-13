namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 58, Length = 14, Description = "g2c_activate_revive_pt")]
public sealed class 激活复活区域 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int X;

	[FieldAttribute(Position = 6, Length = 4)]
	public int Y;

	[FieldAttribute(Position = 10, Length = 4)]
	public int Z;
}
