namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 370, Length = 6, Description = "自动战斗回执")]
public sealed class 自动战斗回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 开关状态;
}
