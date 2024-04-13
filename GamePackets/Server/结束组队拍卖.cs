namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 201, Length = 14, Description = "结束组队拍卖")]
public sealed class 结束组队拍卖 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 拍卖顺序;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 对象编号;
}
