namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 519, Length = 44, Description = "申请更改队伍")]
public sealed class 申请更改队伍 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 成员上限;

    [FieldAttribute(Position = 8, Length = 4)]
    public int 队长编号;
}
