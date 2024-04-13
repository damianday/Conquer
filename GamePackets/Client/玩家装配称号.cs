namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 145, Length = 3, Description = "玩家装配称号")]
public sealed class 玩家装配称号 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public byte TitleID;
}
