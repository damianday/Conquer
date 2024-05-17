namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1282, Length = 2, Description = "账号注册成功")]
public sealed class LauncherRegisterSuccessPacket : GamePacket
{
	public override bool Encrypted => false;
}
