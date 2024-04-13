namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 116, Length = 6, Description = "开启道具消息")]
public sealed class 开启道具消息 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int ObjectID;
}
