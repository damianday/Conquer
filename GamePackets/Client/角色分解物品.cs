namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 86, Length = 5, Description = "角色分解物品")]
public sealed class 角色分解物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 分解数量;
}
