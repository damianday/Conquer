namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 566, Length = 6, Description = "逐出师门应答")]
public sealed class 逐出师门应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
