namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 76, Length = 6, Description = "双倍经验变动")]
public sealed class SyncExperienceRatePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ExperienceRate;
}
