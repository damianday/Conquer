namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 529, Length = 39, Description = "UnknownS3")]
public sealed class UnknownS3 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 37)]
    public byte[] Data = new byte[37];
}
