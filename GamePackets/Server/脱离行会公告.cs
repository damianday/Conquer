namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 601, Length = 6, Description = "脱离行会公告")]
public sealed class 脱离行会公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
