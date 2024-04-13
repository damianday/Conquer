namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 48, Length = 7, Description = "角色拾取物品")]
public sealed class 角色拾取物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 物品编号;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 未知参数;
}
