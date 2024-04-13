namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 105, Length = 8, Description = "切换战斗姿态", Broadcast = true)]
public sealed class SwitchBattleStancePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 姿态编号;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte 触发动作;
}
