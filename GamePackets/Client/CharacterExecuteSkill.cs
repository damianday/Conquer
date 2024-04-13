using System.Drawing;

namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 34, Length = 16, Description = "释放技能")]
public sealed class CharacterExecuteSkill : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte ActionID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int TargetID;

    [FieldAttribute(Position = 10, Length = 4)]
    public Point Location;

    [FieldAttribute(Position = 14, Length = 2)]
    public ushort Height;
}
