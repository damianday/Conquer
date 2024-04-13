namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 606, Length = 6, Description = "申请结盟应答")]
public sealed class 申请结盟应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;
}
