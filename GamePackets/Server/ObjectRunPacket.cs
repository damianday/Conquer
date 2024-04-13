using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 47, Length = 12, Description = "角色跑动", Broadcast = true)]
public sealed class ObjectRunPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort MovementSpeed;

    [FieldAttribute(Position = 8, Length = 4)]
    public Point Position;
}