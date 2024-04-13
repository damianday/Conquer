namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 120, Length = 0, Description = "同步BUFF列表")]
public sealed class 同步状态列表 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
