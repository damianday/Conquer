namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 522, Length = 38, Description = "添加好友关注")]
public sealed class 添加好友关注 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 对象名字;
}
