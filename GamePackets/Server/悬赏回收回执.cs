namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 173, Length = 10, Description = "悬赏回收回执")]
public sealed class 悬赏回收回执 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 物品编号;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 物品位置;
}
