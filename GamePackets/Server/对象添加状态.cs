namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 116, Length = 21, Description = "添加BUFF", Broadcast = true)]
public sealed class 对象添加状态 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort Buff编号;

    [FieldAttribute(Position = 8, Length = 4)]
    public int Buff索引;

    [FieldAttribute(Position = 12, Length = 4)]
    public int Buff来源;

    [FieldAttribute(Position = 16, Length = 4)]
    public int 持续时间;

    [FieldAttribute(Position = 20, Length = 1)]
    public byte Buff层数;
}
