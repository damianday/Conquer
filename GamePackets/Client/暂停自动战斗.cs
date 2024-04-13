namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 269, Length = 6, Description = "暂停自动战斗")]
public sealed class 暂停自动战斗 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 自动战斗;
}
