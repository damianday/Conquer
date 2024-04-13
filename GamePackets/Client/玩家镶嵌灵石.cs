namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 68, Length = 7, Description = "玩家镶嵌灵石")]
public sealed class 玩家镶嵌灵石 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 装备类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 装备孔位;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte 灵石类型;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 灵石位置;
}
