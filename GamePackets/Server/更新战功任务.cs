namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 377, Length = 6, Description = "更新战功任务")]
public sealed class 更新战功任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 任务编号;

	[FieldAttribute(Position = 4, Length = 2)]
	public ushort 任务进度;
}
