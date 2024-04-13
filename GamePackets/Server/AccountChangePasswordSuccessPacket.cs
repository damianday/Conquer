namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1284, Length = 2, Description = "密码修改成功")]
public sealed class AccountChangePasswordSuccessPacket : GamePacket
{
	public override bool Encrypted => false;
}
