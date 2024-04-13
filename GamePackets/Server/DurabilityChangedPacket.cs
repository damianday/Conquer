namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 143, Length = 8, Description = "装备持久改变")]
public sealed class DurabilityChangedPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte Grid;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte Location;

    [FieldAttribute(Position = 4, Length = 4)]
    public int Durability;
}
