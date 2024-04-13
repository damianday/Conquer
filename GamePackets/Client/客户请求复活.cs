namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 24, Length = 3, Description = "玩家请求复活")]
public sealed class 客户请求复活 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 复活方式;
}
