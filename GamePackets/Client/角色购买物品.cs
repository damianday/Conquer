namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 49, Length = 12, Description = "角色购买物品")]
public sealed class 角色购买物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 商店编号;

    [FieldAttribute(Position = 6, Length = 6)]
    public int 物品位置;

    [FieldAttribute(Position = 10, Length = 2)]
    public ushort 购入数量;
}
