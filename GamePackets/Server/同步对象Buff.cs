namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 71, Length = 36, Description = "同步对象Buff")]
public sealed class 同步对象Buff : GamePacket
{
    [FieldAttribute(Position = 2, Length = 34)]
    public byte[] Description;
}
