namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 379, Length = 142, Description = "同步主题礼包")]
public sealed class 同步主题礼包 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 140)]
	public byte[] 购买数据;
}
