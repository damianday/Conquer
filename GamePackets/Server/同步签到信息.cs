namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 288, Length = 5, Description = "同步签到信息")]
public sealed class 同步签到信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
	public byte 签到次数;

	[FieldAttribute(Position = 3, Length = 1)]
	public bool 能否签到;

	[FieldAttribute(Position = 4, Length = 1)]
	public bool 开启签到;
}
