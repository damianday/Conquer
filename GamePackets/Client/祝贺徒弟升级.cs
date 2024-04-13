namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 545, Length = 0, Description = "祝贺徒弟升级(已弃用)")]
public sealed class 祝贺徒弟升级 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 0)]
    public byte[] 字节数据;
}
