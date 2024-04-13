namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 585, Length = 0, Description = "查看行会列表")]
public sealed class 同步行会列表 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 字节数据;
}
