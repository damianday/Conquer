namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 167, Length = 0, Description = "Npcc交互结果")]
public sealed class 同步交互结果 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 8, Length = 0)]
    public byte[] Description;
}
