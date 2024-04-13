namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 202, Length = 6, Description = "开始狩猎任务")]
public sealed class 开始狩猎任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 8)]
	public byte[] 未知参数;
}
