namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1291, Length = 6, Description = "程序提示信息")]
public sealed class AccountSendUpdateInfoFailPacket : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int HintCode;

	public override bool Encrypted => false;
}
