namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 8, Length = 8, Description = "获取真实IP地址")]
public sealed class 获取真实IP地址 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 1)]
	public byte IPAddress1;

	[FieldAttribute(Position = 5, Length = 1)]
	public byte IPAddress2;

	[FieldAttribute(Position = 6, Length = 1)]
	public byte IPAddress3;

	[FieldAttribute(Position = 7, Length = 1)]
	public byte IPAddress4;
}
