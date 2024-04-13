namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 221, Length = 10, Description = "g2c_update_achievement_variable")]
public sealed class 更新成就变量 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 序号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 序号2;
}
