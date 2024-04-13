namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 102, Length = 12, Description = "装备铭文刻印")]
public sealed class 装备铭文刻印 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 解锁参数;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备部位;

    [FieldAttribute(Position = 4, Length = 4)]
    public int 物品编号;

    [FieldAttribute(Position = 8, Length = 4)]
    public int 铭文索引;
}
