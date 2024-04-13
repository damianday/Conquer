namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 198, Length = 10, Description = "角色重铸装备")]
public sealed class 角色重铸装备 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 装备部位;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 未知参数;
}
