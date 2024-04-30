namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 350, Length = 14, Description = "同步技能计数")]
public sealed class 同步技能计数 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte SkillCount;

    [FieldAttribute(Position = 10, Length = 4)]
    public int SkillCooldown;
}
