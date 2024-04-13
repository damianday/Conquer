namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 602, Length = 3, Description = "脱离行会应答")]
public sealed class 脱离行会应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public byte 脱离方式;
}
