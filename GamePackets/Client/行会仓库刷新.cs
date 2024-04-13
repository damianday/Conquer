namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 164, Length = 3, Description = "行会仓库刷新")]
public sealed class 行会仓库刷新 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 仓库页面;
}
