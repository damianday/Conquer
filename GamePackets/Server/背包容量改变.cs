namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 137, Length = 4, Description = "背包容量改变")]
public sealed class 背包容量改变 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte Grid;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte Capacity;
}
