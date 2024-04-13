namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 366, Length = 4, Description = "传永武技签到")]
public sealed class 传永武技签到 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 签到次数;

	[FieldAttribute(Position = 3, Length = 1)]
	public bool 能否签到;
}
