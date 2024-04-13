namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 164, Length = 6, Description = "玩家接取任务")]
public sealed class 玩家接取任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 任务编号;
}
