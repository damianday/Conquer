namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 260, Length = 22, Description = "玩家合无相石")]
public sealed class 玩家合无相石 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 物品位置;

	[FieldAttribute(Position = 18, Length = 4)]
	public int 一键合成;
}
