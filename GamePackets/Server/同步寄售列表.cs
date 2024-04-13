namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 677, Length = 0, Description = "同步寄售列表")]
public sealed class 同步寄售列表 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 1)]
	public byte 消息类型;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte 道具数量;

	[FieldAttribute(Position = 6, Length = 0)]
	public byte[] 道具字节;
}
