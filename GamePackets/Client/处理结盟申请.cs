namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 570, Length = 7, Description = "处理结盟申请")]
public sealed class 处理结盟申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 处理类型;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 行会编号;
}
