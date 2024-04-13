namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 522, Length = 51, Description = "同步队员信息")]
public sealed class 同步队员信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int TeamID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 10, Length = 4)]
    public int CurrentLevel;

    [FieldAttribute(Position = 14, Length = 4)]
    public int MaxHP;

    [FieldAttribute(Position = 18, Length = 4)]
    public int MaxMP;

    [FieldAttribute(Position = 22, Length = 4)]
    public int CurrentHP;

    [FieldAttribute(Position = 26, Length = 4)]
    public int CurrentMP;

    [FieldAttribute(Position = 30, Length = 4)]
    public int CurrentMapID;

    [FieldAttribute(Position = 34, Length = 4)]
    public int RouteID;

    [FieldAttribute(Position = 38, Length = 4)]
    public int CurrentPositionX;

    [FieldAttribute(Position = 42, Length = 4)]
    public int CurrentPositionY;

    [FieldAttribute(Position = 46, Length = 4)]
    public int CurrentHeight;

    [FieldAttribute(Position = 50, Length = 4)]
    public byte AttackMode;
}
