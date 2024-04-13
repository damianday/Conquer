namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 135, Length = 0, Description = "拾取物品")]
public sealed class 玩家拾取物品 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int 角色编号;

    [FieldAttribute(Position = 17, Length = 4)]
    public byte[] 物品描述;
}
