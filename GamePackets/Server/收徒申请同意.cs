namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 559, Length = 6, Description = "同意收徒申请")]
public sealed class 收徒申请同意 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
