namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 198, Length = 0, Description = "添加拍卖物品")]
public sealed class 添加拍卖物品 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 4)]
	public int 拍卖顺序;

	[FieldAttribute(Position = 8, Length = 0)]
	public byte[] 物品描述;
}
