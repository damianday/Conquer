namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 599, Length = 7, Description = "行会禁言公告")]
public sealed class 行会禁言公告 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 禁言状态;
}
