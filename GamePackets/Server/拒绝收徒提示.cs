namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 561, Length = 6, Description = "拒绝收徒提示")]
public sealed class 拒绝收徒提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
