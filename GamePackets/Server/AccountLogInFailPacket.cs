namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1281, Length = 0, Description = "账号登录失败")]
public sealed class AccountLogInFailPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] ErrorMessage;

	public override bool Encrypted => false;
}
