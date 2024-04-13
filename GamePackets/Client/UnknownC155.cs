namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 155, Length = 6, Description = "UnknownC1")]
public sealed class UnknownC155 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort U1;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort U2;
}
