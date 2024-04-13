namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 45, Length = 4, Description = "角色使用物品")]
public sealed class 角色使用物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;
}
