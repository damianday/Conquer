namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 643, Length = 10, Description = "申请敌对应答")]
public sealed class 申请敌对应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 需要资金;
}
