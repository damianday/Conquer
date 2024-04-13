namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 157, Length = 6, Description = "AcceptRewardPacket")]
public sealed class AcceptRewardPacket : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int TaskID;
}
