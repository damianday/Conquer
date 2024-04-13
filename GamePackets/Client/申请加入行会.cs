namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 556, Length = 31, Description = "申请加入行会")]
public sealed class 申请加入行会 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 行会编号;

    [FieldAttribute(Position = 6, Length = 25)]
    public string 行会名字;
}
