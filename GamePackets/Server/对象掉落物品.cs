using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 150, Length = 0, Description = "掉落物品")]
public sealed class 对象掉落物品 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 8, Length = 4)]
    public int MapID;

    [FieldAttribute(Position = 12, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 16, Length = 2)]
    public ushort Height;

    [FieldAttribute(Position = 18, Length = 4)]
    public int ItemID;

    [FieldAttribute(Position = 22, Length = 4)]
    public int Quantity;

    [FieldAttribute(Position = 30, Length = 4)]
    public int OwnerPlayerId;
}
