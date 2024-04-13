namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 89, Length = 22, Description = "角色合成勋章")]
public sealed class 角色合成勋章 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 合成模板;

	[FieldAttribute(Position = 6, Length = 8)]
	public byte[] 未知参数;

	[FieldAttribute(Position = 14, Length = 8)]
	public byte[] 合成参数;
}
