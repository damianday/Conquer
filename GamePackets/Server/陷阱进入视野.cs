using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 123, Length = 30, Description = "陷阱出现")]
public sealed class 陷阱进入视野 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 8, Length = 4)]
    public int CasterID;

    [FieldAttribute(Position = 12, Length = 2)]
    public ushort TrapID;

    [FieldAttribute(Position = 14, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 18, Length = 2)]
    public ushort Height;

    [FieldAttribute(Position = 20, Length = 2)]
    public ushort Duration;

    [FieldAttribute(Position = 22, Length = 2)]
    public ushort 未知参数;

    [FieldAttribute(Position = 24, Length = 4)]
    public Point 未知坐标;

    [FieldAttribute(Position = 28, Length = 2)]
    public ushort 未知高度;
}
