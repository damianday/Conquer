namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 81, Length = 14, Description = "升级武器高级")]
public sealed class 升级武器高级 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 6)]
	public byte[] 首饰组;

	[FieldAttribute(Position = 8, Length = 6)]
	public byte[] 材料组;
}
