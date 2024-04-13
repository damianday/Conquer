namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1003, Length = 6, Description = "删除角色")]
public sealed class 客户删除角色 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
