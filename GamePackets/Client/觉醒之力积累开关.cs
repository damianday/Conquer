namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 226, Length = 3, Description = "觉醒之力积累开关")]
public sealed class 觉醒之力积累开关 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public bool 开关;
}
