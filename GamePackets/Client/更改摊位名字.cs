namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 109, Length = 35, Description = "更改摊位名字")]
public sealed class 更改摊位名字 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 33)]
    public string 摊位名字;
}
