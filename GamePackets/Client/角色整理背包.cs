namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 58, Length = 3, Description = "角色整理背包")]
public sealed class 角色整理背包 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 背包类型;
}
