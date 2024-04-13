namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 251, Length = 6, Description = "解除玩家屏蔽")]
public sealed class 解除玩家屏蔽 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
