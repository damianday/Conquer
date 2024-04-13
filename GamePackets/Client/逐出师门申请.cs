namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 543, Length = 6, Description = "逐出师门申请")]
public sealed class 逐出师门申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
