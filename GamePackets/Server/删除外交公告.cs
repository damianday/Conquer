namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 610, Length = 7, Description = "删除外交公告")]
public sealed class 删除外交公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 外交类型;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 行会编号;
}
