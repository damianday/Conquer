namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 92, Length = 5, Description = "角色删除技能")]
public sealed class 角色删除技能 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 技能编号;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 未知参数;
}
