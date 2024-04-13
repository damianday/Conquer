namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 347, Length = 6, Description = "g2c_wrestle_best_rank")]
public sealed class 摔跤最好排名 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 数据;
}
