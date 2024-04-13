namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 325, Length = 27, Description = "龙卫属性激活状态")]
public sealed class 龙卫属性激活状态 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 属性位置;

	[FieldAttribute(Position = 3, Length = 24)]
	public byte[] 数据;
}
