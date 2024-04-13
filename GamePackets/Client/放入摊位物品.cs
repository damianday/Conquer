namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 107, Length = 11, Description = "放入摊位物品")]
public sealed class 放入摊位物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 放入位置;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品容器;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 5, Length = 2)]
    public ushort 物品数量;

    [FieldAttribute(Position = 7, Length = 1)]
    public int 物品价格;
}
