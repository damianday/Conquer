namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 115, Length = 6, Description = "玩家放弃任务")]
public sealed class 玩家放弃任务 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 任务编号;
}
