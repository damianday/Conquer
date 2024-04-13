namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 609, Length = 38, Description = "添加外交公告")]
public sealed class 添加外交公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 外交类型;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 行会编号;

    [FieldAttribute(Position = 7, Length = 4)]
    public int 外交时间;

    [FieldAttribute(Position = 11, Length = 1)]
    public byte 行会等级;

    [FieldAttribute(Position = 12, Length = 1)]
    public byte 行会人数;

    [FieldAttribute(Position = 13, Length = 25)]
    public string 行会名字;
}
