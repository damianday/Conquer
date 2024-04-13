namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 141, Length = 10, Description = "队友拍卖竞价")]
public sealed class 组队拍卖竞价 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 拍卖顺序;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 当前竞价;
}
