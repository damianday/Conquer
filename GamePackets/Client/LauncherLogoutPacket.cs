namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1297, Length = 2, Description = "账号注销登录")]
public sealed class LauncherLogoutPacket : GamePacket
{
	public override bool Encrypted => false;
}
