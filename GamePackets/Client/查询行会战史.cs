namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 178, Length = 3, Description = "查询行会战史")]
public sealed class 查询行会战史 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 宠物模式;
}
