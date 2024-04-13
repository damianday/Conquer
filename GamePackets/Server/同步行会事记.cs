namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 621, Length = 0, Description = "更多行会事记")]
public sealed class 同步行会事记 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 字节数据;
}
