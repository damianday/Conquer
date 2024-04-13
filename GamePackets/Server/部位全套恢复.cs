namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 342, Length = 0, Description = "全套部位恢复")]
public sealed class 部位全套恢复 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 数据;
}
