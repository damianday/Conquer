namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 596, Length = 35, Description = "行会邀请应答")]
public sealed class 行会邀请应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 应答类型;

    [FieldAttribute(Position = 3, Length = 32)]
    public string 对象名字;
}
