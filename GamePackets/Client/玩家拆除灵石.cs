namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 69, Length = 5, Description = "玩家拆除灵石")]
public sealed class 玩家拆除灵石 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 装备类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 装备孔位;
}
