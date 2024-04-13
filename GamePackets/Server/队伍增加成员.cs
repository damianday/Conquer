namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 518, Length = 45, Description = "队伍增加成员")]
public sealed class 队伍增加成员 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int TeamID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 10, Length = 32)]
    public string 对象名字;

    [FieldAttribute(Position = 42, Length = 1)]
    public byte 对象性别;

    [FieldAttribute(Position = 43, Length = 1)]
    public byte 对象职业;

    [FieldAttribute(Position = 44, Length = 1)]
    public byte 在线离线;
}
