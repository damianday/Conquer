namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 614, Length = 0, Description = "行会仓库刷新")]
public sealed class 刷新行会仓库 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 字节数据;
}