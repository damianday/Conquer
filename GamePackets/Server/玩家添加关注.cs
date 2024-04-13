namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 533, Length = 39, Description = "添加关注")]
public sealed class 玩家添加关注 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 对象名字;

    [FieldAttribute(Position = 38, Length = 1)]
    public bool 是否好友;
}
