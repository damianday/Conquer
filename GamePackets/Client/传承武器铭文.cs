namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 71, Length = 6, Description = "传承武器铭文")]
public sealed class 传承武器铭文 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 来源类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 来源位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 目标类型;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 目标位置;
}
