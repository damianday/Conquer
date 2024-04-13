namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 156, Length = 6, Description = "玩家比较成就")]
public sealed class 玩家比较成就 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
