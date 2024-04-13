namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 613, Length = 0, Description = "仓库刷新应答")]
public sealed class 仓库刷新应答 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 字节数据;
}
