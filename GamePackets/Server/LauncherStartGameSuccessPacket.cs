namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1286, Length = 0, Description = "进入游戏成功")]
public sealed class LauncherStartGameSuccessPacket : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] Ticket;

	public override bool Encrypted => false;
}
