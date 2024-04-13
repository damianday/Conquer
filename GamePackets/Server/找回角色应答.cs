namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1007, Length = 6, Description = "找回角色回应")]
public sealed class 找回角色应答 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
