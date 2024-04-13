namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 664, Length = 10, Description = "行会攻城登记")]
public sealed class 行会攻城登记 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;

    [FieldAttribute(Position = 6, Length = 4)]
    public int U1;
}
