namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 110, Length = 3, Description = "更改摊位外观")]
public sealed class 更改摊位外观 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 外观编号;
}
