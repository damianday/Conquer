namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 553, Length = 6, Description = "拒绝拜师申请")]
public sealed class 拜师申请拒绝 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
