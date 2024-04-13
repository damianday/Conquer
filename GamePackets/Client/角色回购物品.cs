namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 52, Length = 3, Description = "角色回购物品")]
public sealed class 角色回购物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 物品位置;
}
