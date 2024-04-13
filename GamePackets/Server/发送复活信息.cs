namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 57, Length = 55, Description = "复活信息(无此封包不会弹出复活框)")]
public sealed class 发送复活信息 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 等待时间;

    [FieldAttribute(Position = 6, Length = 4)]
    public int 凶手编号;

    [FieldAttribute(Position = 10, Length = 45)]
    public byte[] Description;

    public 发送复活信息()
    {
        Description = new byte[45]
        {
            0, 0,
            0, 0, 0, 0, 0, 0, 0, 1, 161, 134,
            1, 0, 1, 0, 0, 0, 2, 1, 0, 0,
            0, 100, 0, 0, 0, 3, 0, 0, 0, 0,
            0, 0, 0, 0, 4, 0, 0, 0, 0, 0,
            0, 0, 0
        };
    }
}
