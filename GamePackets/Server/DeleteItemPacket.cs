namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 130, Length = 4, Description = "删除物品")]
public sealed class DeleteItemPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte Grid;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte Position;
}
