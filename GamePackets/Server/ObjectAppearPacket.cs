using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 60, Length = 20, Description = "对象出现, 适用于对象主动进入视野")]
public sealed class ObjectAppearPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte Effect;

    [FieldAttribute(Position = 3, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte 现身姿态;

    [FieldAttribute(Position = 8, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 12, Length = 2)]
    public ushort Height;

    [FieldAttribute(Position = 14, Length = 2)]
    public ushort Direction;

    [FieldAttribute(Position = 16, Length = 1)]
    public byte HealthPercent;

    [FieldAttribute(Position = 17, Length = 1)]
    public byte 坐骑编号;

    [FieldAttribute(Position = 18, Length = 1)]
    public byte 传承之力;

    [FieldAttribute(Position = 19, Length = 1)]
    public byte 补充参数;
}
