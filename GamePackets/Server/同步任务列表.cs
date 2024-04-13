namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 20, Length = 0, Description = "g2c_sync_quest_list")]
public sealed class 同步任务列表 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] Description;
}
