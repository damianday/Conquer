namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 618, Length = 23, Description = "添加公会事记")]
public sealed class AddGuildLogPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte LogType;

    [FieldAttribute(Position = 3, Length = 4)]
    public int Param1;

    [FieldAttribute(Position = 7, Length = 4)]
    public int Param2;

    [FieldAttribute(Position = 11, Length = 4)]
    public int Param3;

    [FieldAttribute(Position = 15, Length = 4)]
    public int Param4;

    [FieldAttribute(Position = 19, Length = 4)]
    public int LogTime;
}
