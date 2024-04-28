namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 149, Length = 30, Description = "货币数量变动")]
public sealed class SyncCurrencyPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort Currency;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort Param2;

    [FieldAttribute(Position = 6, Length = 4)]
    public int Amount; // TODO: Change to uint

    [FieldAttribute(Position = 10, Length = 4)]
    public uint Param4;

    [FieldAttribute(Position = 14, Length = 4)]
    public uint Param5;

    [FieldAttribute(Position = 18, Length = 4)]
    public uint Param6;

    [FieldAttribute(Position = 22, Length = 4)]
    public uint Param7;

    [FieldAttribute(Position = 26, Length = 4)]
    public uint Param8;
}
