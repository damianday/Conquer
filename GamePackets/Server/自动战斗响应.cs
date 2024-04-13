namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 369, Length = 6, Description = "自动战斗响应")]
public sealed class 自动战斗响应 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 响应;
}
