namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 87, Length = 94, Description = "同步师门奖励(师徒通用)")]
public sealed class 同步师门奖励 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 92)]
    public byte[] Description;
}
