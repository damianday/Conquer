namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 267, Length = 8, Description = "双铭文位切换")]
public sealed class 双铭文位切换 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 2)]
    public ushort 当前栏位;

    [FieldAttribute(Position = 4, Length = 2)]
    public ushort 第一铭文;

    [FieldAttribute(Position = 6, Length = 2)]
    public ushort 第二铭文;
}
