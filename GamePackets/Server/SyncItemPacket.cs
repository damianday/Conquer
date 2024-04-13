namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 128, Length = 0, Description = "玩家物品变动")]
public sealed class SyncItemPacket : GamePacket
{
    [FieldAttribute(Position = 4, Length = 0)]
    public byte[] Description;
}
