namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 60, Length = 4, Description = "角色勋章洗练")]
public sealed class 角色勋章洗练 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 未知参数;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 物品位置;
}
