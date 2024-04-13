namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 657, Length = 6, Description = "同步元宝数量")]
public sealed class 同步元宝数量 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 4)]
    public int 元宝数量; // TODO: Convert to uint
}
