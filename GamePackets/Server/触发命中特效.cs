namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 97, Length = 28, Description = "触发命中特效(技能信息,目标,血量,反馈)", Broadcast = true)]
public sealed class 触发命中特效 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte SkillLevel;

    [FieldAttribute(Position = 9, Length = 1)]
    public byte InscriptionID;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte ActionID;

    [FieldAttribute(Position = 11, Length = 1)]
    public byte CastId;

    [FieldAttribute(Position = 12, Length = 1)]
    public byte 陷阱编号;

    [FieldAttribute(Position = 13, Length = 4)]
    public int TargetID;

    [FieldAttribute(Position = 17, Length = 4)]
    public int SkillDamage;

    [FieldAttribute(Position = 21, Length = 2)]
    public ushort ParryDamage;

    [FieldAttribute(Position = 25, Length = 2)]
    public ushort SkillFeedback;

    [FieldAttribute(Position = 27, Length = 1)]
    public byte 附加特效;
}
