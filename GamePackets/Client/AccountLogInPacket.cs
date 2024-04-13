namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1280, Length = 0, Description = "申请账号登录")]
public sealed class AccountLogInPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] LoginInformation;

	public override bool Encrypted => false;
}
