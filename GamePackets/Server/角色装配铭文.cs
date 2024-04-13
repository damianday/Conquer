namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 101, Length = 6, Description = "角色装配铭文")]
public sealed class 角色装配铭文 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte InscriptionID;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte SkillLevel;
}
