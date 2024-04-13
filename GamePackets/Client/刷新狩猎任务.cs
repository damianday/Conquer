namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 206, Length = 10, Description = "刷新狩猎任务")]
public sealed class 刷新狩猎任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 8)]
	public byte[] 未知参数;
}
