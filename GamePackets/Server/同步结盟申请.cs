namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 607, Length = 0, Description = "查看结盟申请")]
public sealed class 同步结盟申请 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 字节描述;
}
