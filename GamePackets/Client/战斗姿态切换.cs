namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 40, Length = 4, Description = "战斗姿态切换")]
public sealed class 战斗姿态切换 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 姿态编号;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 触发动作;

    static 战斗姿态切换()
    {
    }
}
