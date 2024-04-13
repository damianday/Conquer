namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 86, Length = 10, Description = "同步战力")]
public sealed class 同步玩家战力 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int CombatPower;
}
