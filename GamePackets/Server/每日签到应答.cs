namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 289, Length = 3, Description = "玩家每日签到")]
public sealed class 每日签到应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 签到次数;
}
