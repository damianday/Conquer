namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 66, Length = 58, Description = "同步Npcc数据扩展(宠物)")]
public sealed class 同步扩展数据 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort MonID;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte Grade;

    [FieldAttribute(Position = 11, Length = 1)]
    public byte CurrentLevel;

    [FieldAttribute(Position = 12, Length = 4)]
    public int MaxHP;

    [FieldAttribute(Position = 16, Length = 1)]
    public byte ObjectType;

    [FieldAttribute(Position = 17, Length = 1)]
    public byte PetLevel;

    [FieldAttribute(Position = 18, Length = 4)]
    public int MasterID;

    [FieldAttribute(Position = 22, Length = 36)]
    public string MasterName;
}
