namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 593, Length = 54, Description = "行会加入成员")]
public sealed class 行会加入成员 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 对象名字;

    [FieldAttribute(Position = 38, Length = 1)]
    public byte 对象职位;

    [FieldAttribute(Position = 39, Length = 1)]
    public byte 对象等级;

    [FieldAttribute(Position = 40, Length = 1)]
    public byte 对象职业;

    [FieldAttribute(Position = 41, Length = 1)]
    public byte 当前地图;
}
