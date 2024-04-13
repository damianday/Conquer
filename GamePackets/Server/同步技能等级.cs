namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 127, Length = 7, Description = "同步技能等级数据")]
public sealed class 同步技能等级 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort CurrentExperience;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte CurrentLevel;
}
