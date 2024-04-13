namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 280, Length = 4, Description = "g2c_activity_change")]
public sealed class 更新活动日程 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 日程进度;
}
