namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 559, Length = 34, Description = "邀请加入行会")]
public sealed class 邀请加入行会 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 32)]
    public string 对象名字;
}
