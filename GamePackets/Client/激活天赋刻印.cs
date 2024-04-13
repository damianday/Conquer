namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 259, Length = 10, Description = "激活天赋刻印")]
public sealed class 激活天赋刻印 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 天赋位置;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 刻印位置;
}
