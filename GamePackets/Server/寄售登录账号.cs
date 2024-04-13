namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 676, Length = 68, Description = "同步珍宝数量")]
public sealed class 寄售登录账号 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 消息类型;

	[FieldAttribute(Position = 3, Length = 65)]
	public string 登录账号;
}
