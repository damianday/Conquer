namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 25, Length = 8, Description = "同步师门信息")]
public sealed class 同步师门信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 限制时间;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 师门参数;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte 师门推送;
}
