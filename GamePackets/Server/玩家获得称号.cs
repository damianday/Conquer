namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 213, Length = 7, Description = "玩家获得称号")]
public sealed class 玩家获得称号 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 称号编号;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 剩余时间;
}
