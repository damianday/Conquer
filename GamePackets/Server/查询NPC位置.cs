namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 705, Length = 26, Description = "m2c_query_npc_pos")]
public sealed class 查询NPC位置 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 地图编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 对象编号;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 状态标志;

	[FieldAttribute(Position = 14, Length = 4)]
	public int X;

	[FieldAttribute(Position = 18, Length = 4)]
	public int Y;

	[FieldAttribute(Position = 22, Length = 4)]
	public int Z;
}
