namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 527, Length = 6, Description = "添加仇人")]
public sealed class 玩家添加仇人 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
