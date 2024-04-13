namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 567, Length = 6, Description = "转移会长职位")]
public sealed class 转移会长职位 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
