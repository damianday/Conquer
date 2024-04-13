namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 616, Length = 4, Description = "仓库转出应答")]
public sealed class 仓库转出应答 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 仓库页面;

	[FieldAttribute(Position = 3, Length = 1)]
	public byte 仓库位置;
}