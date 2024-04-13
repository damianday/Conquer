namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 520, Length = 40, Description = "发送组队申请")]
public sealed class 发送组队申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 组队方式;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte 对象职业;

    [FieldAttribute(Position = 8, Length = 32)]
    public string 对象名字;
}
