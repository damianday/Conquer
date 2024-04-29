namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 317, Length = 3, Description = "坐骑面板回执")]
public sealed class 坐骑面板回执 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte MountID;
}
