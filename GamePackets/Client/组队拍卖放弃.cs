namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 142, Length = 6, Description = "组队拍卖放弃")]
public sealed class 组队拍卖放弃 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 拍卖顺序;
}
