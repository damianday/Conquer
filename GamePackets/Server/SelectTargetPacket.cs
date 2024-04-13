namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 121, Length = 10, Description = "角色选中目标")]
public sealed class SelectTargetPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int ObjectID;

    [FieldAttribute(Position = 6, Length = 4)]
    public int TargetObjectID;
}
