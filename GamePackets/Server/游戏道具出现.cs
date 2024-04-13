using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 152, Length = 19, Description = "游戏道具出现")]
public sealed class 游戏道具出现 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 模板编号;

    [FieldAttribute(Position = 10, Length = 4)]
    public Point 对象坐标;

    [FieldAttribute(Position = 14, Length = 2)]
    public ushort 对象高度;

    [FieldAttribute(Position = 16, Length = 2)]
    public ushort 对象方向;

    [FieldAttribute(Position = 18, Length = 1)]
    public bool 能否交互;
}
