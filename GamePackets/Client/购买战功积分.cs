namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 277, Length = 6, Description = "购买战功积分")]
public sealed class 购买战功积分 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 购买类型;
}
