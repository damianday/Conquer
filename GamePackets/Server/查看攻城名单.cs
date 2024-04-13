namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 286, Length = 0, Description = "查看攻城名单")]
public sealed class 查看攻城名单 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 字节描述;
}
