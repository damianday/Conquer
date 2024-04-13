namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 166, Length = 8, Description = "行会仓库转出")]
public sealed class 行会仓库转出 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 仓库页面;

    [FieldAttribute(Position = 3, Length = 1)]
    public byte 仓库位置;

    [FieldAttribute(Position = 4, Length = 1)]
    public byte 目标容器;

    [FieldAttribute(Position = 6, Length = 1)]
    public byte 目标位置;
}
