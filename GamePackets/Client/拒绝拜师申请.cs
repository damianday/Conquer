namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 536, Length = 6, Description = "拒绝拜师申请")]
public sealed class 拒绝拜师申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
