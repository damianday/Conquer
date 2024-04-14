using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 46, Length = 12, Description = "角色走动", Broadcast = true)]
public sealed class ObjectWalkPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort Speed;

    [FieldAttribute(Position = 8, Length = 4)]
    public Point Position;
}
