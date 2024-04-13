namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1285, Length = 0, Description = "账号注册失败")]
public sealed class AccountChangePasswordFailPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] ErrorMessage;

	public override bool Encrypted => false;
}
