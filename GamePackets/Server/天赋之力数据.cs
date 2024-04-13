namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 360, Length = 82, Description = "天赋之力数据")]
public sealed class 天赋之力数据 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 0)]
	public byte[] 数据数组;
}
