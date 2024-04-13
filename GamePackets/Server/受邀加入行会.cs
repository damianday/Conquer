namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 595, Length = 63, Description = "受邀加入行会")]
public sealed class 受邀加入行会 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 对象编号;

    [FieldAttribute(Position = 6, Length = 32)]
    public string 对象名字;

    [FieldAttribute(Position = 38, Length = 25)]
    public string 行会名字;
}
