namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1004, Length = 6, Description = "彻底删除角色")]
public sealed class 彻底删除角色 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
