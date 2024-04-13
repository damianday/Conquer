using System.Drawing;

namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 12, Length = 176, Description = "同步角色数据(地图.坐标.职业.性别.等级...)")]
public sealed class SyncCharacterInfoPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int AwakeningExp;

    [FieldAttribute(Position = 10, Length = 4)]
    public int CurrentExperience;

    [FieldAttribute(Position = 16, Length = 2)]
    public ushort 未知参数169;

    [FieldAttribute(Position = 18, Length = 4)]
    public int MaxExperience;

    [FieldAttribute(Position = 26, Length = 4)]
    public int MaxAwakeningExp;

    [FieldAttribute(Position = 34, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 38, Length = 4)]
    public int MapID;

    [FieldAttribute(Position = 58, Length = 4)]
    public int 经验上限;

    [FieldAttribute(Position = 62, Length = 4)]
    public int ExperienceRate;

    [FieldAttribute(Position = 70, Length = 4)]
    public int PKPoint;

    [FieldAttribute(Position = 90, Length = 4)]
    public int CurrentTime;

    [FieldAttribute(Position = 110, Length = 2)]
    public ushort 鸿运点数;

    [FieldAttribute(Position = 114, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 118, Length = 2)]
    public ushort Height;

    [FieldAttribute(Position = 120, Length = 2)]
    public ushort Direction;

    [FieldAttribute(Position = 124, Length = 2)]
    public ushort MaxLevel;

    [FieldAttribute(Position = 128, Length = 2)]
    public ushort SpecialRepairDiscount;

    [FieldAttribute(Position = 130, Length = 1)]
    public byte Job;

    [FieldAttribute(Position = 131, Length = 1)]
    public byte Gender;

    [FieldAttribute(Position = 132, Length = 1)]
    public byte Level;

    [FieldAttribute(Position = 138, Length = 1)]
    public byte Distance;

    [FieldAttribute(Position = 139, Length = 1)]
    public byte AutoBattleMode;

    [FieldAttribute(Position = 140, Length = 1)]
    public byte AttackMode;

    [FieldAttribute(Position = 141, Length = 1)]
    public byte 未知参数139;

    [FieldAttribute(Position = 143, Length = 1)]
    public bool 是否灰名;


    // TODO: Rename baove
    /*[FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int MapID;

    [FieldAttribute(Position = 10, Length = 4)]
    public int RouteID; // 16777217

    [FieldAttribute(Position = 14, Length = 1)]
    public byte Job;

    [FieldAttribute(Position = 15, Length = 1)]
    public byte Gender;

    [FieldAttribute(Position = 16, Length = 1)]
    public byte Level;

    [FieldAttribute(Position = 17, Length = 45)]
    public byte[] Unknown1 = new byte[45];

    [FieldAttribute(Position = 62, Length = 4)]
    public Point Position;

    [FieldAttribute(Position = 66, Length = 2)]
    public ushort Height;

    [FieldAttribute(Position = 68, Length = 2)]
    public ushort Direction;

    [FieldAttribute(Position = 70, Length = 1)]
    public byte Distance = 1;

    [FieldAttribute(Position = 71, Length = 1)]
    public byte 挂机 = 2;

    [FieldAttribute(Position = 72, Length = 8)]
    public long MaxExperience; // 所需经验

    [FieldAttribute(Position = 84, Length = 8)]
    public long 经验上限 = 9223372032559808512L;

    [FieldAttribute(Position = 88, Length = 4)]
    public int ExperienceRate;

    [FieldAttribute(Position = 92, Length = 4)]
    public int Unknown3 = 0;

    [FieldAttribute(Position = 96, Length = 4)]
    public int PKPoint;

    [FieldAttribute(Position = 100, Length = 1)]
    public byte AttackMode;

    [FieldAttribute(Position = 101, Length = 16)]
    public byte[] Unknown4 = new byte[16];

    [FieldAttribute(Position = 117, Length = 4)]
    public int CurrentTime;

    [FieldAttribute(Position = 121, Length = 14)]
    public byte[] Unknown5 = new byte[14];

    [FieldAttribute(Position = 135, Length = 2)]
    public ushort 开放等级;

    [FieldAttribute(Position = 137, Length = 8)]
    public byte[] Unknown6 = new byte[8];

    [FieldAttribute(Position = 145, Length = 8)]
    public long CurrentExperience; //当前经验

    [FieldAttribute(Position = 153, Length = 1)]
    public bool BrownName = false;

    [FieldAttribute(Position = 154, Length = 4)]
    public int AwakeningExp;

    [FieldAttribute(Position = 158, Length = 4)]
    public int Unknown8;

    [FieldAttribute(Position = 162, Length = 4)]
    public int MaxAwakeningExp;

    [FieldAttribute(Position = 166, Length = 4)]
    public int Unknown7;

    [FieldAttribute(Position = 170, Length = 2)]
    public ushort 特修折扣;*/
}
