namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 265, Length = 25, Description = "开始自动战斗")]
public sealed class 开始自动战斗 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public bool 自动战斗;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 战斗范围;

    [FieldAttribute(Position = 4, Length = 1)]
    public bool 开启空闲使用道具;

    [FieldAttribute(Position = 5, Length = 2)]
    public short 空闲时间;

    [FieldAttribute(Position = 7, Length = 4)]
    public int 道具ID;

    [FieldAttribute(Position = 11, Length = 1)]
    public byte Unk1;

    [FieldAttribute(Position = 12, Length = 4)]
    public int 技能ID;

    [FieldAttribute(Position = 16, Length = 1)]
    public bool 开启自动拾取;

    [FieldAttribute(Position = 17, Length = 1)]
    public byte 拾取范围;

    [FieldAttribute(Position = 18, Length = 1)]
    public bool 开启预留背包;

    [FieldAttribute(Position = 19, Length = 1)]
    public byte 预留格数;

    [FieldAttribute(Position = 20, Length = 1)]
    public bool 优先战斗;

    [FieldAttribute(Position = 21, Length = 1)]
    public bool 不捡取他人装备;

    [FieldAttribute(Position = 22, Length = 1)]
    public bool 不抢怪;

    [FieldAttribute(Position = 23, Length = 1)]
    public bool Unk2;
}
