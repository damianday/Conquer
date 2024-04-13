namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 534, Length = 6, Description = "取消关注")]
public sealed class 玩家取消关注 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
