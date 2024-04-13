namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 75, Length = 46, Description = "角色经验变动")]
public sealed class CharacterExperienceChangePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ExperienceGained;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 今日增加;

    [FieldAttribute(Position = 10, Length = 4)]
    public int 经验上限;

    [FieldAttribute(Position = 14, Length = 4)]
    public int ExperienceRate;

    [FieldAttribute(Position = 18, Length = 4)]
    public int CurrentExperience;

    [FieldAttribute(Position = 22, Length = 4)]
    public int Unkown1;

    [FieldAttribute(Position = 26, Length = 4)]
    public int MaxExperience;

    [FieldAttribute(Position = 30, Length = 4)]
    public int Unkown2;

    [FieldAttribute(Position = 34, Length = 4)]
    public int AwakeningExperienceGained;

    [FieldAttribute(Position = 38, Length = 4)]
    public int MaxAwakeningExperience;
}
