namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 138, Length = 13, Description = "同步角色外形", Broadcast = true)]
public sealed class 同步角色外形 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte WearType;

    [FieldAttribute(Position = 8, Length = 4)]
    public int ItemID;

    [FieldAttribute(Position = 12, Length = 1)]
    public byte UpgradeCount;
}
