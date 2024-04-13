namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 691, Length = 16, Description = "UnknownS1")]
public sealed class UnknownS1 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort U1;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort U2;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort U3;

    [FieldAttribute(Position = 8, Length = 2)]
    public ushort U4;

    [FieldAttribute(Position = 10, Length = 2)]
    public ushort U5;

    [FieldAttribute(Position = 12, Length = 2)]
    public ushort U6;

    [FieldAttribute(Position = 14, Length = 2)]
    public ushort U7;
}
