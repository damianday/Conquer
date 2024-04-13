namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 122, Length = 0, Description = "选中目标详情")]
public sealed class SelectTargetDetailsPacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 8, Length = 4)]
    public int CurrentHP;

    [FieldAttribute(Position = 12, Length = 4)]
    public int CurrentMP;

    [FieldAttribute(Position = 16, Length = 4)]
    public int MaxHP;

    [FieldAttribute(Position = 20, Length = 4)]
    public int MaxMP;

    [FieldAttribute(Position = 25, Length = 1)]
    public byte[] BuffDescription;
}
