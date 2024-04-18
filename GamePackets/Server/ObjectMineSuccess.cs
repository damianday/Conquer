using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 59, Length = 12, Description = "玩家挖矿成功")]
public sealed class ObjectMineSuccess : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public int ObjectID;

	[FieldAttribute(Position = 6, Length = 4)]
	public Point Location;

	[FieldAttribute(Position = 10, Length = 2)]
	public ushort ActionTime;
}
