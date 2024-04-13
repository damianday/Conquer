namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 584, Length = 28, Description = "申请行会敌对")]
public sealed class 申请行会敌对 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 敌对时间;

    [FieldAttribute(Position = 3, Length = 25)]
    public string 行会名字;
}
