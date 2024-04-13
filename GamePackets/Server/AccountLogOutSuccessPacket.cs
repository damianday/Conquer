namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1288, Length = 0, Description = "账号注销登录")]
public sealed class AccountLogOutSuccessPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] ErrorMessage;

	public override bool Encrypted => false;
}
