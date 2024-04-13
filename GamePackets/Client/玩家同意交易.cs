namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 148, Length = 6, Description = "玩家同意交易")]
public sealed class 玩家同意交易 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
