namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1005, Length = 6, Description = "找回角色")]
public sealed class 客户找回角色 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
