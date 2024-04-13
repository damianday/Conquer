namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1008, Length = 6, Description = "永久删除角色回应")]
public sealed class 永久删除角色 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
