namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 1006, Length = 6, Description = "进入游戏")]
public sealed class EnterGamePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
