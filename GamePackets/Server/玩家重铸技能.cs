namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 294, Length = 10, Description = "玩家重铸技能")]
public sealed class 玩家重铸技能 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int 通知结果;

	[FieldAttribute(Position = 6, Length = 4)]
	public int 返回编号;
}
