namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 104, Length = 10, Description = "体力变动飘字")]
public sealed class IncreaseHealthIndicatorPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int HealthAmount;
}
