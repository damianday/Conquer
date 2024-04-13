namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1288, Length = 6, Description = "下载游戏文件")]
public sealed class 下载游戏文件 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 文件序号;

	public override bool Encrypted => false;
}
