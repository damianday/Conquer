namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 89, Length = 9, Description = "角色学习技能")]
public sealed class 角色学习技能 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte SkillLevel;
}
