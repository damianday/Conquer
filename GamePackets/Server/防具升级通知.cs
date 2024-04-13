namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 343, Length = 6, Description = "防具升级通知")]
public sealed class 防具升级通知 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 通知结果;
}
