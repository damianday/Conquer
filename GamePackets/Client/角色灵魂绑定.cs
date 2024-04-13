namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 101, Length = 3, Description = "角色灵魂绑定")]
public sealed class 角色灵魂绑定 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 未知参数;
}
