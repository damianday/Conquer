namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 565, Length = 6, Description = "离开师门提示")]
public sealed class 离开师门提示 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
