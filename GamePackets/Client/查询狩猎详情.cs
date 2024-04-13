namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 205, Length = 10, Description = "查询狩猎详情")]
public sealed class 查询狩猎详情 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 8)]
	public byte[] 未知参数;
}
