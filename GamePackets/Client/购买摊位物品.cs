namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 112, Length = 17, Description = "购买摊位物品")]
public sealed class 购买摊位物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 7, Length = 2)]
    public ushort 购买数量;
}
