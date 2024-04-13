namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1006, Length = 6, Description = "删除角色回应")]
public sealed class 删除角色应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
