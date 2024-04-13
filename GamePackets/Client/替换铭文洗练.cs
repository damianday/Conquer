namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 76, Length = 8, Description = "替换铭文洗练")]
public sealed class 替换铭文洗练 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 装备类型;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 装备位置;

    [FieldAttribute(Position = 4, Length = 4)]
    public int 物品编号;
}
