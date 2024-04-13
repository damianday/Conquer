namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 219, Length = 5, Description = "坐骑御兽拖动")]
public sealed class 坐骑御兽拖动 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 御兽栏位;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 坐骑编号;
}
