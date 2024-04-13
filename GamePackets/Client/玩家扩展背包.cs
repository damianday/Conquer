namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 57, Length = 4, Description = "玩家扩展背包")]
public sealed class 玩家扩展背包 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 扩展大小;
}
