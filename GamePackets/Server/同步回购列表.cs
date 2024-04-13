namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 133, Length = 0, Description = "同步回购列表")]
public sealed class 同步回购列表 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
