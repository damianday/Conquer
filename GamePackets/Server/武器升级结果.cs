namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 258, Length = 3, Description = "武器升级结果")]
public sealed class 武器升级结果 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 升级结果;
}
