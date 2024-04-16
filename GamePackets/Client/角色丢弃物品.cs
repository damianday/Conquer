namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 47, Length = 6, Description = "角色丢弃物品")]
public sealed class 角色丢弃物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte Grid;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte Location;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort Quantity;
}
