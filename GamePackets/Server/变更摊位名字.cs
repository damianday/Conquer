namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 159, Length = 39, Description = "更改摊位名字", Broadcast = true)]
public sealed class 变更摊位名字 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 33)]
    public string 摊位名字;
}
