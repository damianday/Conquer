namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 46, Length = 8, Description = "角色拆分物品")]
public sealed class 角色拆分物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 当前背包;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 物品位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 目标背包;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 目标位置;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort 拆分数量;
}
