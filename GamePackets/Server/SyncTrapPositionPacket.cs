using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 124, Length = 18, Description = "陷阱移动位置", Broadcast = true)]
public sealed class SyncTrapPositionPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int TrapID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort MoveSpeed;

    [FieldAttribute(Position = 8, Length = 4)]
    public int 未知参数;

    [FieldAttribute(Position = 12, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 16, Length = 4)]
    public ushort Height;
}
