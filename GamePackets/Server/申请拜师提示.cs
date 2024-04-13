namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 551, Length = 6, Description = "申请拜师提示")]
public sealed class 申请拜师提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
