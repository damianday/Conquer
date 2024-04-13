namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 149, Length = 30, Description = "货币数量变动")]
public sealed class 货币数量变动 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte Currency;

    [FieldAttribute(Position = 6, Length = 4)]
    public int Amount; // TODO: Change to uint
}
