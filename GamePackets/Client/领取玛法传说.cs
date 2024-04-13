namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 189, Length = 3, Description = "领取玛法传说")]
public sealed class 领取玛法传说 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 领取编号;
}
