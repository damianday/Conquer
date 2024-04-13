namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 165, Length = 8, Description = "行会仓库转入")]
public sealed class 行会仓库转入 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 原来容器;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 原来位置;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 仓库页面;

    [FieldAttribute(Position = 7, Length = 1)]
    public byte 仓库位置;
}
