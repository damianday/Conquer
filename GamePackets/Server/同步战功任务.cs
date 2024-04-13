namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 376, Length = 0, Description = "同步战功任务")]
public sealed class 同步战功任务 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 任务描述;
}
