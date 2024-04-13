namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1290, Length = 0, Description = "请求短信验证")]
public sealed class AccountSMSVerificationRequestPacket : GamePacket
{
	[FieldAttribute(Position = 2, Length = 0)]
	public string MobilePhoneNumber;

	public override bool Encrypted => false;
}
