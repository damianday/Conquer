namespace GamePackets.Client;

[PacketInfo(Source = PacketSource.Client, ID = 572, Length = 5, Description = "更改存储权限")]
public sealed class 更改存储权限 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
    public byte 行会职位;

    [FieldAttribute(Position = 3, Length = 2)]
    public ushort 权限标志;
}
