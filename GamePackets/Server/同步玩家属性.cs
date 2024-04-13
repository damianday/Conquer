namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 34, Length = 0, Description = "同步玩家属性")]
public sealed class 同步玩家属性 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 8, Length = 4)]
    public int 属性数量;
}
