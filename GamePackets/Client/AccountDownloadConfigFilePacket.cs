namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1289, Length = 6, Description = "下载配置文件")]
public sealed class AccountDownloadConfigFilePacket : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 文件序号;

	public override bool Encrypted => false;
}
