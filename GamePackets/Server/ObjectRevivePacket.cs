namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 56, Length = 7, Description = "玩家角色复活")]
public sealed class ObjectRevivePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte ResurrectionMode;
}