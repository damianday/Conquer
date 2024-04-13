namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 64, Length = 133, Description = "同步玩家外观")]
public sealed class SyncObjectRolePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte Unknown1;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte Job;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte Gender;

    [FieldAttribute(Position = 9, Length = 1)]
    public byte HairStyle;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte HairColor;

    [FieldAttribute(Position = 11, Length = 1)]
    public byte FaceStyle;

    [FieldAttribute(Position = 12, Length = 1)]
    public byte Unknown2;

    [FieldAttribute(Position = 13, Length = 1)]
    public byte Unknown3;

    [FieldAttribute(Position = 14, Length = 1)]
    public int PKLevel;

    [FieldAttribute(Position = 15, Length = 4)]
    public byte[] Unknown4 = new byte[4];

    [FieldAttribute(Position = 19, Length = 1)]
    public byte 武器等级;

    [FieldAttribute(Position = 20, Length = 4)]
    public int 身上武器;

    [FieldAttribute(Position = 24, Length = 4)]
    public int 身上衣服;

    [FieldAttribute(Position = 28, Length = 4)]
    public int 身上披风;

    [FieldAttribute(Position = 32, Length = 4)]
    public int MaxHP;

    [FieldAttribute(Position = 36, Length = 4)]
    public int MaxMP;

    [FieldAttribute(Position = 40, Length = 5)]
    public byte[] Unknown5 = new byte[5];

    [FieldAttribute(Position = 45, Length = 1)]
    public byte 称号编号;

    [FieldAttribute(Position = 46, Length = 4)]
    public int 外观时间;

    [FieldAttribute(Position = 50, Length = 1)]
    public byte 摆摊状态;

    [FieldAttribute(Position = 51, Length = 0)]
    public string 摊位名字;

    [FieldAttribute(Position = 84, Length = 45)]
    public string Name;

    [FieldAttribute(Position = 118, Length = 4)]
    public int GuildID;
}
