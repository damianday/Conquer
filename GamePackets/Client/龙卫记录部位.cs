namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 222, Length = 4, Description = "龙卫记录部位")]
public sealed class 龙卫记录部位 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 记录部位;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 记录序号;
}
