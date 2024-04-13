namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 608, Length = 6, Description = "处理结盟申请")]
public sealed class 结盟申请应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;
}
