namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 552, Length = 6, Description = "同意拜师申请")]
public sealed class 拜师申请通过 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
