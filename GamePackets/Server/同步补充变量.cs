namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 187, Length = 13, Description = "同步客户变量")]
public sealed class 同步补充变量 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 变量类型;

    [FieldAttribute(Position = 3, Length = 2)]
    public ushort 变量索引;

    [FieldAttribute(Position = 5, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 9, Length = 4)]
    public int 变量内容;
}
