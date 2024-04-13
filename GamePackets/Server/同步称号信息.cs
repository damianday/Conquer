namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 24, Length = 0, Description = "同步称号信息")]
public sealed class 同步称号信息 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
