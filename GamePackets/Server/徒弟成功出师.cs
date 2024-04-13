namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 573, Length = 6, Description = "徒弟成功出师")]
public sealed class 徒弟成功出师 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
