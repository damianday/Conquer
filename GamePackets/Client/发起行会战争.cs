namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 579, Length = 27, Description = "发起行会战争")]
public sealed class 发起行会战争 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 25)]
    public string 行会名字;
}
