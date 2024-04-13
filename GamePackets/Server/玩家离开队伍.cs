namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 517, Length = 6, Description = "玩家离开队伍")]
public sealed class 玩家离开队伍 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 队伍编号;
}
