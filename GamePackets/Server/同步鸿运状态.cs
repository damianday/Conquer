namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 385, Length = 6, Description = "同步鸿运状态")]
public sealed class 同步鸿运状态 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 状态编号;
}
