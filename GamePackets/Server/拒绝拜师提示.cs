namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 554, Length = 6, Description = "拒绝拜师提示")]
public sealed class 拒绝拜师提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
