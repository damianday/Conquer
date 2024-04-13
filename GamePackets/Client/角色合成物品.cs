namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 87, Length = 6, Description = "角色合成物品")]
public sealed class 角色合成物品 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 物品编号;
}
