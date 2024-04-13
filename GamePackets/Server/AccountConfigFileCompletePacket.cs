namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1298, Length = 2, Description = "文件更新完成")]
public sealed class AccountConfigFileCompletePacket : GamePacket
{
	public override bool Encrypted => false;
}
