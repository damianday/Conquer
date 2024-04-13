namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 125, Length = 5, Description = "技能升级")]
public sealed class SyncSkillLevelPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte SkillLevel;
}
