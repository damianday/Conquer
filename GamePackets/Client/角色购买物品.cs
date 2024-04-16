namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 49, Length = 12, Description = "角色购买物品")]
public sealed class 角色购买物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int StoreID;

    [FieldAttribute(Position = 6, Length = 6)]
    public int Location;

    [FieldAttribute(Position = 10, Length = 2)]
    public ushort Quantity;
}
