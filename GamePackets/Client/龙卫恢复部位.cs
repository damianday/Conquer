namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 223, Length = 4, Description = "龙卫恢复部位")]
public sealed class 龙卫恢复部位 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 记录部位;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 记录序号;
}
