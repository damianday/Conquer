namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 657, Length = 6, Description = "同步元宝数量")]
public sealed class SyncIngotsPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int Amount; // TODO: Convert to uint
}
