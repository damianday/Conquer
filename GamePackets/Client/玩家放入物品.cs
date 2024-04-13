namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 150, Length = 6, Description = "玩家放入物品")]
public sealed class 玩家放入物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 放入位置;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 放入物品;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 物品容器;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 物品位置;
}
