namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 618, Length = 23, Description = "添加公会事记")]
public sealed class 添加公会事记 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 事记类型;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 第一参数;

    [FieldAttribute(Position = 7, Length = 4)]
    public int 第二参数;

    [FieldAttribute(Position = 11, Length = 4)]
    public int 第三参数;

    [FieldAttribute(Position = 15, Length = 4)]
    public int 第四参数;

    [FieldAttribute(Position = 19, Length = 4)]
    public int 事记时间;
}
