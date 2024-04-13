namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 534, Length = 6, Description = "玩家申请拜师")]
public sealed class 玩家申请拜师 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
