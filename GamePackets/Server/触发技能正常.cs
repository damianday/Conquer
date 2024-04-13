namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 96, Length = 0, Description = "触发技能正常(技能信息,目标,反馈,耗时)", Broadcast = true)]
public sealed class 触发技能正常 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort 发送特殊标记;

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
    public byte SegmentID;

    [FieldAttribute(Position = 14, Length = 0)]
    public byte[] HitDescription;

    public 触发技能正常()
    {
        SegmentID = 1;
        HitDescription = new byte[1];
    }
}
