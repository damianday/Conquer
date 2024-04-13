namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 250, Length = 38, Description = "玩家屏蔽目标")]
public sealed class 玩家屏蔽目标 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 对象名字;
}
