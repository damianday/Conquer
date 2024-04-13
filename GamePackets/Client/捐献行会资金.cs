namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 163, Length = 6, Description = "捐献行会资金")]
public sealed class 捐献行会资金 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 金币数量;
}
