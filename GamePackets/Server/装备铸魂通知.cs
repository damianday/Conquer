namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 275, Length = 4, Description = "装备铸魂通知")]
public sealed class 装备铸魂通知 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 2)]
	public ushort 返回结果;
}
