namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 29, Length = 0, Description = "g2c_sync_achievement_list")]
public sealed class 同步成就列表 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] Description;
}
