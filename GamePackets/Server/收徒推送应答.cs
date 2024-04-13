namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 544, Length = 3, Description = "收徒推送应答")]
public sealed class 收徒推送应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public bool 收徒推送;
}