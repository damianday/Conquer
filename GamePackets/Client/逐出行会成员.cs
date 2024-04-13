namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 566, Length = 6, Description = "逐出行会成员")]
public sealed class 逐出行会成员 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
