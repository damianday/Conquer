namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 600, Length = 7, Description = "变更职位公告")]
public sealed class 变更职位公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 对象职位;
}
