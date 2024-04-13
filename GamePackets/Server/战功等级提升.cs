namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 378, Length = 6, Description = "战功等级提升")]
public sealed class 战功等级提升 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 未知参数一;

	[FieldAttribute(Position = 4, Length = 2)]
	public ushort 未知参数二;
}
