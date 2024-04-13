namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 218, Length = 10, Description = "g2c_complete_achievement")]
public sealed class 成就完成通知 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int U1;

    [FieldAttribute(Position = 6, Length = 4)]
    public int U2;
}
