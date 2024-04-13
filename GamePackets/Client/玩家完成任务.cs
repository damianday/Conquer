namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 114, Length = 10, Description = "玩家完成任务")]
public sealed class 玩家完成任务 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 任务编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 未知标识;
}
