namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 276, Length = 4, Description = "灵魂绑定通知")]
public sealed class 灵魂绑定通知 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 返回结果;
}
