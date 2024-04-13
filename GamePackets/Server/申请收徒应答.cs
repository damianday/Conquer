namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 557, Length = 6, Description = "玩家申请收徒")]
public sealed class 申请收徒应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
