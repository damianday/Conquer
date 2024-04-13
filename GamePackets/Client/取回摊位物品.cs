namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 108, Length = 3, Description = "取回摊位物品")]
public sealed class 取回摊位物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 取回位置;
}
