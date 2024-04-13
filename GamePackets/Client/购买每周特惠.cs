namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 255, Length = 6, Description = "购买每周特惠")]
public sealed class 购买每周特惠 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 礼包编号;
}
