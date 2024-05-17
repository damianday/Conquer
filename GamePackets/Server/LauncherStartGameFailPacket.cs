namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1287, Length = 0, Description = "进入游戏失败")]
public sealed class LauncherStartGameFailPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] ErrorMessage;

	public override bool Encrypted => false;
}
