namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 216, Length = 0, Description = "放入交易物品")]
public sealed class TradeAddItemPacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte Location;

    [FieldAttribute(Position = 9, Length = 1)]
    public byte 放入物品;

    [FieldAttribute(Position = 10, Length = 0)]
    public byte[] Description;
}
