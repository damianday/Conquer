namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 520, Length = 6, Description = "查询队伍信息")]
public sealed class 查询队伍信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
