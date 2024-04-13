namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 236, Length = 6, Description = "任务传送封包")]
public sealed class 任务传送封包 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 任务编号;
}
