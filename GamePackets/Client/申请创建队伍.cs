namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 521, Length = 7, Description = "申请创建队伍")]
public sealed class 申请创建队伍 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 分配方式;
}
