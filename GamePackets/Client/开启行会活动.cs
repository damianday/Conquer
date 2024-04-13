namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 581, Length = 6, Description = "开启行会活动")]
public sealed class 开启行会活动 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 活动编号;
}
