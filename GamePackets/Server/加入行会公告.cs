namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 623, Length = 27, Description = "加入行会公告")]
public sealed class 加入行会公告 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 25)]
	public string 行会名字;
}