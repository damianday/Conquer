namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 1003, Length = 6, Description = "进入游戏应答")]
public sealed class StartGamePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int CharacterID;
}
