using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 48, Length = 13, Description = "对象角色停止")]
public sealed class ObjectStopPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte U1 = 1;

    [FieldAttribute(Position = 7, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 11, Length = 2)]
    public ushort Height;
}
