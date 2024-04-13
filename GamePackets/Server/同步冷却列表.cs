namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 108, Length = 0, Description = "同步冷却列表")]
public sealed class 同步冷却列表 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
