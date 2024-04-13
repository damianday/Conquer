using System;
using System.Drawing;

namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 30, Length = 6, Description = "玩家开始挖矿")]
public sealed class 玩家开始挖矿 : GamePacket
{
	[FieldAttribute(Position = 2, Length = 4)]
	public Point 挖掘坐标;
}
