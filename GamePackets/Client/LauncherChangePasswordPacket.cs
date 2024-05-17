namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1284, Length = 0, Description = "申请密码修改")]
public sealed class LauncherChangePasswordPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] AccountInformation;

	public override bool Encrypted => false;
}
