namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 241, Length = 6, Description = "角色防具升级")]
public sealed class 角色防具升级 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 装备部位;
}
