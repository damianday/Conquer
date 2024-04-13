namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 540, Length = 6, Description = "拒绝收徒申请")]
public sealed class 拒绝收徒申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
