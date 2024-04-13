namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 572, Length = 6, Description = "祝贺徒弟应答")]
public sealed class 祝贺徒弟应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 徒弟编号;
}
