namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 339, Length = 0, Description = "同步奖励找回")]
public sealed class 同步奖励找回 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 字节描述;
}
