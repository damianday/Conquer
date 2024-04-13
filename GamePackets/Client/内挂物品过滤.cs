namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 267, Length = 282, Description = "内挂物品过滤")]
public sealed class 内挂物品过滤 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 280)]
	public byte[] Description;
}
