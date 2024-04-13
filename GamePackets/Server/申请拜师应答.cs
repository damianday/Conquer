namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 550, Length = 6, Description = "玩家申请拜师")]
public sealed class 申请拜师应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
