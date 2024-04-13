namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 111, Length = 6, Description = "进入战斗姿态")]
public sealed class 进入战斗姿态 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;
}
