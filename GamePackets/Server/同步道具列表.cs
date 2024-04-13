using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 153, Length = 0, Description = "同步道具列表")]
public sealed class 同步道具列表 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectId;

    [FieldAttribute(Position = 8, Length = 4)]
    public int NPCTemplateId;

    [FieldAttribute(Position = 12, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 16, Length = 2)]
    public ushort Altitude;

    [FieldAttribute(Position = 18, Length = 2)]
    public ushort Direction;

    [FieldAttribute(Position = 20, Length = 1)]
    public bool Interactive;
}
