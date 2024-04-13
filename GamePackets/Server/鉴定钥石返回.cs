namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 364, Length = 3, Description = "鉴定钥石返回")]
public sealed class 鉴定钥石返回 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public bool 是否成功;
}
