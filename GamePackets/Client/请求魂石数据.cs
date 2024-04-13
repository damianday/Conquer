namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 232, Length = 6, Description = "请求魂石数据")]
public sealed class 请求魂石数据 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 对象编号;
}
