using System.Drawing;

namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 14, Length = 10, Description = "上传角色位置")]
public sealed class 上传角色位置 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public Point Location;

    [FieldAttribute(Position = 8, Length = 2)]
    public ushort Height;
}
