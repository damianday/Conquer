namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 200, Length = 6, Description = "灵符兑换历练点")]
public sealed class 灵符兑换历练 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 获得历练点数;
}
