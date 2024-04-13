namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 98, Length = 13, Description = "技能释放中断", Broadcast = true)]
public sealed class 技能释放中断 : GamePacket
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
    public byte SegmentID;
}
