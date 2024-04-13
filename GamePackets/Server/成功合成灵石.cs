namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 252, Length = 3, Description = "玩家合成灵石")]
public sealed class 成功合成灵石 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 灵石状态;
}
