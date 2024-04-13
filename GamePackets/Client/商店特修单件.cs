namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 55, Length = 4, Description = "商店特修单件")]
public sealed class 商店特修单件 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 物品容器;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;
}
