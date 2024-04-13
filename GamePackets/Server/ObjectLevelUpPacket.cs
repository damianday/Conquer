namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 74, Length = 7, Description = "角色等级提升", Broadcast = true)]
public sealed class ObjectLevelUpPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte CurrentLevel;
}
