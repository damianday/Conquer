namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 95, Length = 0, Description = "触发技能扩展(技能信息,目标,反馈,耗时)", Broadcast = true)]
public sealed class 触发技能扩展 : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 8, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte SkillLevel;

    [FieldAttribute(Position = 11, Length = 1)]
    public byte InscriptionID;

    [FieldAttribute(Position = 12, Length = 1)]
    public byte ActionID;

    [FieldAttribute(Position = 13, Length = 1)]
    public byte 技能分段;

    [FieldAttribute(Position = 14, Length = 1)]
    public byte 未知参数;

    [FieldAttribute(Position = 15, Length = 0)]
    public byte[] HitDescription;
}
