using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 39, Length = 17, Description = "进入场景(包括商店/随机卷)")]
public sealed class ObjectEnterScenePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int MapID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int RouteID;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte RouteStatus;

    [FieldAttribute(Position = 11, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 15, Length = 2)]
    public ushort Height;
}
