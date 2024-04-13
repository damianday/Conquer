namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 613, Length = 4, Description = "购买玛法特权")]
public sealed class 购买玛法特权 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 特权类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 购买数量;
}
