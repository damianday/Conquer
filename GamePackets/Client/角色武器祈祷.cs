namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 90, Length = 3, Description = "角色武器祈祷")]
public sealed class 角色武器祈祷 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 未知参数;
}
