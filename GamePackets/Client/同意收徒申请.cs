namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 539, Length = 6, Description = "同意收徒申请")]
public sealed class 同意收徒申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
