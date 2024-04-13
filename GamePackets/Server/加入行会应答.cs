namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 589, Length = 6, Description = "加入行会回应")]
public sealed class 加入行会应答 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 行会编号;
}