namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1296, Length = 0, Description = "下载游戏文件")]
public sealed class 发送游戏文件 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] 文件信息;

	public override bool Encrypted => false;
}
