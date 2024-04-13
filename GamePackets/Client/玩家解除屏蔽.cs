namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 181, Length = 6, Description = "玩家解除屏蔽")]
public sealed class 玩家解除屏蔽 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
