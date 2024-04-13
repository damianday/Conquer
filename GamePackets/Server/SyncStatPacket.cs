namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 77, Length = 7, Description = "同步属性变动")]
public sealed class SyncStatPacket : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte Stat;

    [FieldAttribute(Position = 3, Length = 4)]
    public int Value;
}
