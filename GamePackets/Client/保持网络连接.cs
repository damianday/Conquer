namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1296, Length = 2, Description = "保持网络连接")]
public sealed class 保持网络连接 : GamePacket
{
	public override bool Encrypted => false;
}
