namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 242, Length = 4, Description = "玩家喝修复油")]
public sealed class 玩家喝修复油 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;
}
