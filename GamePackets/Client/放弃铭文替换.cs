namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 78, Length = 11, Description = "放弃铭文替换")]
public sealed class 放弃铭文替换 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 装备类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备位置;

    [FieldAttribute(Position = 4, Length = 4)]
    public int 物品编号;
}
