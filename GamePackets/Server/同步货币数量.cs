namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 148, Length = 0, Description = "同步货币数量")]
public sealed class 同步货币数量 : GamePacket
{
    [FieldAttribute(Position = 5, Length = 0)]
    public byte[] Description;
}
