namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 517, Length = 6, Description = "申请离开队伍")]
public sealed class 申请离开队伍 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
