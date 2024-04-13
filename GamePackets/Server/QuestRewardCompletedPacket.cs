namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 220, Length = 6, Description = "QuestRewardCompletedPacket")]
public sealed class QuestRewardCompletedPacket : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int QuestId;
}
