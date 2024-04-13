namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 535, Length = 38, Description = "对方关注自己")]
public sealed class 对方关注自己 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 对象名字;
}
