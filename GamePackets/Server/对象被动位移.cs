using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 49, Length = 17, Description = "被动位移", Broadcast = true)]
public sealed class 对象被动位移 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 第一标记;

    [FieldAttribute(Position = 7, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 11, Length = 2)]
    public ushort 第二标记;

    [FieldAttribute(Position = 13, Length = 2)]
    public ushort 位移朝向;

    [FieldAttribute(Position = 15, Length = 2)]
    public ushort 位移速度;
}
