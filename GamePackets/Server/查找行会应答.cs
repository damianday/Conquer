namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 587, Length = 231, Description = "查找行会应答")]
public sealed class 查找行会应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 229)]
    public byte[] Description;
}
