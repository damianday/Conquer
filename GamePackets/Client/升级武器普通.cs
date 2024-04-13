namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 79, Length = 14, Description = "升级武器普通")]
public sealed class 升级武器普通 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 6)]
    public byte[] 首饰组;

    [FieldAttribute(Position = 8, Length = 6)]
    public byte[] 材料组;
}
