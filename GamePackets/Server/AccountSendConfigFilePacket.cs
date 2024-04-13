namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1297, Length = 0, Description = "发送配置文件")]
public sealed class AccountSendConfigFilePacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] FileInformation;

	public override bool Encrypted => false;
}
