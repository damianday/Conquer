using System.Drawing;

namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 18, Length = 6, Description = "角色走动")]
public sealed class UserRequestWalk : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public Point Location;
}
