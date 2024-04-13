namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 149, Length = 6, Description = "玩家放入金币")]
public sealed class 玩家放入金币 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 金币数量;
}
