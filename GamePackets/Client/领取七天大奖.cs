namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 254, Length = 3, Description = "领取七天奖励")]
public sealed class 领取七天大奖 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 领取天数;
}
