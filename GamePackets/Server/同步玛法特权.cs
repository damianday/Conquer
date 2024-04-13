namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 333, Length = 3, Description = "同步玛法特权")]
public sealed class 同步玛法特权 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 玛法特权;
}
