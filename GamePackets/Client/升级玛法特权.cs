namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 286, Length = 6, Description = "升级玛法特权")]
public sealed class 升级玛法特权 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 特权编号;
}
