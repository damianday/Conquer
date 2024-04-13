namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1282, Length = 0, Description = "申请账号注册")]
public sealed class AccountRegisterPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] RegistrationInformation;

	public override bool Encrypted => false;
}
