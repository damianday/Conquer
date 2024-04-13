namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 134, Length = 3, Description = "切换地图路线")]
public sealed class 切换地图路线 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 地图路线;
}
