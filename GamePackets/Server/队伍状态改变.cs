namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 521, Length = 52, Description = "队伍状态改变")]
public sealed class 队伍状态改变 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 队伍编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 队伍名字;

    [FieldAttribute(Position = 38, Length = 4)]
    public int 成员上限;

    [FieldAttribute(Position = 42, Length = 1)]
    public byte 分配方式;

    [FieldAttribute(Position = 44, Length = 4)]
    public int 队长编号;
}
