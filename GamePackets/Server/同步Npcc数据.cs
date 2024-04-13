namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 65, Length = 16, Description = "同步Npcc数据")]
public sealed class 同步Npcc数据 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort MonID;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte Grade = 3;

    [FieldAttribute(Position = 11, Length = 1)]
    public byte CurrentLevel;

    [FieldAttribute(Position = 12, Length = 4)]
    public int MaxHP;
}
