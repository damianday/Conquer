namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 99, Length = 5, Description = "释放完成, 可以释放下一个技能")]
public sealed class 技能释放完成 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte ActionID;
}
