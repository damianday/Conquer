namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 44, Length = 6, Description = "物品转移/交换/合并/更换装备")]
public sealed class 角色转移物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 当前背包;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 原有位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 目标背包;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 目标位置;
}
