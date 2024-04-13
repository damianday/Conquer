namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 93, Length = 8, Description = "随身修理单件")]
public sealed class 随身修理单件 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 物品容器;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 4, Length = 4)]
    public int 物品编号;
}
