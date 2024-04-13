namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 156, Length = 7, Description = "摆摊状态改变", Broadcast = true)]
public sealed class 摆摊状态改变 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 摊位状态;
}
