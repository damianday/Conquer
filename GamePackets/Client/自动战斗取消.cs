namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 268, Length = 6, Description = "自动战斗响应")]
public sealed class 自动战斗取消 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 自动战斗;
}
