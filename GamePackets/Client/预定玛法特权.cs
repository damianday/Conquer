namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 217, Length = 3, Description = "预定玛法特权")]
public sealed class 预定玛法特权 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 特权类型;
}
