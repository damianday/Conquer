namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 254, Length = 3, Description = "玩家取下灵石")]
public sealed class 成功取下灵石 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 灵石状态;
}
