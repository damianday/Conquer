namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 163, Length = 11, Description = "售出摊位物品")]
public sealed class 售出摊位物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 售出数量;

    [FieldAttribute(Position = 7, Length = 4)]
    public int 售出收益;
}
