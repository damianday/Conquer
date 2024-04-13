namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 18, Length = 0, Description = "同步技能信息")]
public sealed class SyncSkillInfoPacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int UnknownInt32;

    [FieldAttribute(Position = 8, Length = 4)]
    public int SkillCount;

    [FieldAttribute(Position = 12, Length = 0)]
    public byte[] Description;

    //[FieldAttribute(Position = 4, Length = 0)]
    //public byte[] Description;
}
