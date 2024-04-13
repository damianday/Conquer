namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 573, Length = 6, Description = "行会仓库移动")]
public sealed class 行会仓库移动 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 原有页面;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 原有位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 目标页面;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 目标位置;
}
