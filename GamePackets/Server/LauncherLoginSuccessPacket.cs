namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1280, Length = 0, Description = "账号登录成功")]
public sealed class LauncherLoginSuccessPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] ServerListInformation;

	public override bool Encrypted => false;
}
