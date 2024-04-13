namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 227, Length = 4, Description = "觉醒技能升级")]
public sealed class 觉醒技能升级 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 技能编号;
}
