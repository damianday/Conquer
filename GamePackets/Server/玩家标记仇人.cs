namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 538, Length = 6, Description = "添加仇人")]
public sealed class 玩家标记仇人 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
