namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 526, Length = 43, Description = "查询对象队伍信息回应")]
public sealed class 查询队伍应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 队伍编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 队伍名字;

    [FieldAttribute(Position = 39, Length = 4)]
    public int 队长编号;
}
