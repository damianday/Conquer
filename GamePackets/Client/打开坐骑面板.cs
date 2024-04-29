namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 218, Length = 3, Description = "打开坐骑面板")]
public sealed class 打开坐骑面板 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte MountID;
}
