namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 68, Length = 0, Description = "同步黑名单表")]
public sealed class 同步黑名单表 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
