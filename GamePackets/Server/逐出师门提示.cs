namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 567, Length = 6, Description = "逐出师门提示")]
public sealed class 逐出师门提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
