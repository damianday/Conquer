namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 178, Length = 7, Description = "更新玩家威望")]
public sealed class 更新玩家威望 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 1)]
	public byte 更新序号;

	[FieldAttribute(Position = 3, Length = 4)]
	public int 更新数值;
}
