namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 200, Length = 18, Description = "通知组队拍卖")]
public sealed class 通知组队拍卖 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 拍卖顺序;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 对象编号;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 当前价格;

	[FieldAttribute(Position = 14, Length = 4)]
	public int 重置时间;
}
