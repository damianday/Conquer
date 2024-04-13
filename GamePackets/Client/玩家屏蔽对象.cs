namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 588, Length = 38, Description = "玩家屏蔽对象")]
public sealed class 玩家屏蔽对象 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
