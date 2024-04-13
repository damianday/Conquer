namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 129, Length = 6, Description = "物品转移或交换")]
public sealed class 玩家转移物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 原有容器;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 原有位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 目标容器;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 目标位置;
}
