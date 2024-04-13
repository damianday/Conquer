namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 598, Length = 0, Description = "更改行会公告")]
public sealed class 变更行会公告 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] 字节数据;
}
