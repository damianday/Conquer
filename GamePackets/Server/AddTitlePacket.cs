namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 213, Length = 7, Description = "玩家获得称号")]
public sealed class AddTitlePacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte TitleID;

    [FieldAttribute(Position = 3, Length = 4)]
    public int Duration;
}
