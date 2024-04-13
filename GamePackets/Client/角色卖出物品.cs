namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 50, Length = 6, Description = "角色卖出物品")]
public sealed class 角色卖出物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort 卖出数量;
}
