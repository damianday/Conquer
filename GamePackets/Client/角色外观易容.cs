namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 209, Length = 6, Description = "角色外观易容")]
public sealed class 角色外观易容 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 角色发型;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 角色发色;

	[FieldAttribute(Position = 4, Length = 1)]
	public byte 角色脸型;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte 未知参数;
}
