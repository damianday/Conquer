namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 15, Length = 11, Description = "同步背包大小 仓库 背包 资源包...")]
public sealed class SyncBackpackSizePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte WearingSize;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte InventorySize;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte WarehouseSize;

    [FieldAttribute(Position = 5, Length = 1)]
    public byte u1;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte u2;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte u3;

    [FieldAttribute(Position = 8, Length = 1)]
    public byte u4;

    [FieldAttribute(Position = 9, Length = 1)]
    public byte 资源背包大小;

    [FieldAttribute(Position = 10, Length = 1)]
    public byte u5;
}
