namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 523, Length = 6, Description = "取消好友关注")]
public sealed class 取消好友关注 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
