namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 118, Length = 21, Description = "BUFF变动", Broadcast = true)]
public sealed class 对象状态变动 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort Buff编号;

    [FieldAttribute(Position = 8, Length = 4)]
    public int Buff索引;

    [FieldAttribute(Position = 12, Length = 1)]
    public byte 当前层数;

    [FieldAttribute(Position = 13, Length = 4)]
    public int 剩余时间;

    [FieldAttribute(Position = 17, Length = 4)]
    public int 持续时间;
}
