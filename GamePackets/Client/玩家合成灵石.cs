namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 67, Length = 10, Description = "玩家合成灵石")]
public sealed class 玩家合成灵石 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 物品编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 幸运符数;
}
