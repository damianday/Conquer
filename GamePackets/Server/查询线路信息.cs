namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 543, Length = 0, Description = "查询线路信息")]
public sealed class 查询线路信息 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}