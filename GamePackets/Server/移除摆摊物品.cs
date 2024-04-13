namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 158, Length = 3, Description = "移除摆摊物品")]
public sealed class 移除摆摊物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 取回位置;
}
