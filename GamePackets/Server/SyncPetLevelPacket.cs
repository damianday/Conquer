namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 72, Length = 7, Description = "同步宠物等级", Broadcast = true)]
public sealed class SyncPetLevelPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte PetLevel;
}
