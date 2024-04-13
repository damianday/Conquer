namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 216, Length = 0, Description = "放入交易物品")]
public sealed class 放入交易物品 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte 放入位置;

    [FieldAttribute(Position = 9, Length = 1)]
    public byte 放入物品;

    [FieldAttribute(Position = 10, Length = 0)]
    public byte[] 物品描述;
}
