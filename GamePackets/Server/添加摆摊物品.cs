namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 157, Length = 11, Description = "添加摆摊物品")]
public sealed class 添加摆摊物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 放入位置;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 5, Length = 2)]
    public ushort 物品数量;

    [FieldAttribute(Position = 7, Length = 4)]
    public int 物品价格;
}
