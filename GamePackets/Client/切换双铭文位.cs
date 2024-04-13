namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 75, Length = 5, Description = "切换双铭文位")]
public sealed class 切换双铭文位 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 装备类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 操作参数;
}
