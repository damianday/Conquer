namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 74, Length = 5, Description = "解锁双铭文位")]
public sealed class 解锁双铭文位 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 装备类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 操作参数;
}
