namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 199, Length = 15, Description = "开始拍卖物品")]
public sealed class 开始拍卖物品 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 拍卖顺序;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 起拍价格;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 参与人数;
}
