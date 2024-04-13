namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 611, Length = 3, Description = "发送行会通知")]
public sealed class 发送行会通知 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 提醒类型;
}
