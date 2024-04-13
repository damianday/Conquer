namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 88, Length = 20, Description = "角色合成装备")]
public sealed class 角色合成装备 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 合成模板;

	[FieldAttribute(Position = 6, Length = 7)]
	public byte[] 未知参数;

	[FieldAttribute(Position = 13, Length = 7)]
	public byte[] 合成参数;
}
