namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 650, Length = 10, Description = "特权引导寻路")]
public sealed class 特权引导寻路 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 地图编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 对象编号;
}
