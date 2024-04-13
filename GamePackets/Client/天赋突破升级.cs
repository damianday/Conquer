namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 258, Length = 6, Description = "天赋突破升级")]
public sealed class 天赋突破升级 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 天赋位置;
}
