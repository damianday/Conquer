using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 94, Length = 25, Description = "开始释放技能(技能信息,目标,坐标,速率)", Broadcast = true)]
public sealed class 开始释放技能 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort SkillID;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte SkillLevel;

    [FieldAttribute(Position = 9, Length = 1)]
    public byte InscriptionID;

    [FieldAttribute(Position = 10, Length = 4)]
    public int TargetID;

    [FieldAttribute(Position = 14, Length = 4)]
    public Point TargetLocation;

    [FieldAttribute(Position = 18, Length = 2)]
    public ushort TargetHeight;

    [FieldAttribute(Position = 20, Length = 2)]
    public ushort 加速率一;

    [FieldAttribute(Position = 22, Length = 2)]
    public ushort 加速率二;

    [FieldAttribute(Position = 24, Length = 1)]
    public byte ActionID;

    public 开始释放技能()
    {
        加速率一 = 10000;
        加速率二 = 10000;
    }
}
