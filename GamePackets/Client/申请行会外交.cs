namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 568, Length = 29, Description = "申请行会外交")]
public sealed class 申请行会外交 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 外交类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 外交时间;

    [FieldAttribute(Position = 4, Length = 25)]
    public string 行会名字;
}
