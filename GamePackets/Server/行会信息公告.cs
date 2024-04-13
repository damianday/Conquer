namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 588, Length = 0, Description = "行会信息公告")]
public sealed class 行会信息公告 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
