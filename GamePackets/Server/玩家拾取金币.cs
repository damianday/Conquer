namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 136, Length = 10, Description = "拾取金币")]
public sealed class 玩家拾取金币 : GamePacket
{
    [FieldAttribute(Position = 6, Length = 4)]
    public int Amount;
}
