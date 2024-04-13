namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 293, Length = 14, Description = "玩家重铸装备")]
public sealed class 玩家重铸装备 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
	public int 通知结果;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 返回编号;

	[FieldAttribute(Position = 10, Length = 4)]
	public int 未知参数;
}
