namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 547, Length = 0, Description = "查询师门成员(师徒通用)")]
public sealed class 同步师门成员 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 字节数据;
}
