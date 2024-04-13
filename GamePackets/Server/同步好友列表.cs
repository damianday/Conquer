namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 531, Length = 82, Description = "好友上下线")]
public sealed class 同步好友列表 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 32)]
    public string Name;

    [FieldAttribute(Position = 75, Length = 1)]
    public byte Job;

    [FieldAttribute(Position = 76, Length = 1)]
    public byte Gender;

    [FieldAttribute(Position = 77, Length = 1)]
    public byte 上线下线;
}
