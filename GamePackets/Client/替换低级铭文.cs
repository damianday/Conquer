namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 77, Length = 4, Description = "替换低级铭文")]
public sealed class 替换低级铭文 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 装备类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备位置;
}
