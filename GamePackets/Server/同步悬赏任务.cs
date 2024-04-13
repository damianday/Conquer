namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 170, Length = 0, Description = "g2c_sync_refreshed_reward_quest")]
public sealed class 同步悬赏任务 : GamePacket
{
	[FieldAttribute(Position = 4, Length = 0)]
	public byte[] Description;
}
