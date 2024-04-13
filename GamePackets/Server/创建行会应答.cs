namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 584, Length = 27, Description = "创建行会应答")]
public sealed class 创建行会应答 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 25)]
	public string 行会名字;
}