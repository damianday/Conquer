namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 560, Length = 6, Description = "拒绝收徒申请")]
public sealed class 收徒申请拒绝 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
