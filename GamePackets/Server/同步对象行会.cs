namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 223, Length = 10, Description = "同步对象行会", Broadcast = true)]
public sealed class 同步对象行会 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 行会编号;
}
