namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 568, Length = 6, Description = "收徒成功提示")]
public sealed class 收徒成功提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
