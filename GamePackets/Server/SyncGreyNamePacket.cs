namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 82, Length = 7, Description = "玩家名字变灰", Broadcast = true)]
public sealed class SyncGreyNamePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public bool Greyed;
}
