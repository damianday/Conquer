using System.Drawing;

namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 17, Length = 6, Description = "角色跑动")]
public sealed class UserRequestRun : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4, Reversed = true)]
    public Point Location;
}
