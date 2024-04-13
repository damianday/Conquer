namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 583, Length = 7, Description = "处理解敌申请")]
public sealed class 处理解敌申请 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 回应类型;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 行会编号;
}
