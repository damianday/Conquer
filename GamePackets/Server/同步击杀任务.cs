namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 21, Length = 774, Description = "g2c_sync_kill_npc_quest")]
public sealed class 同步击杀任务 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 772)]
	public byte[] Description;
}
