using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 41, Length = 23, Description = "切换地图(回城/过图/传送)")]
public sealed class MapChangedPacket : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int MapID;

    [FieldAttribute(Position = 10, Length = 4)]
    public int RouteID;

    [FieldAttribute(Position = 14, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 18, Length = 2)]
    public ushort Height;

    [FieldAttribute(Position = 20, Length = 2)]
    public ushort Direction;

    [FieldAttribute(Position = 22, Length = 1)]
    public byte 未知参数3;
}
