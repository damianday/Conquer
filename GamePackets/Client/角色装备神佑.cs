namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 244, Length = 6, Description = "角色装备神佑")]
public sealed class 角色装备神佑 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 装备部位;
}
