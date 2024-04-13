namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 553, Length = 7, Description = "查看行会列表")]
public sealed class 查看行会列表 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 查看方式;

    [FieldAttribute(Position = 3, Length = 4)]
    public int 行会编号;
}
