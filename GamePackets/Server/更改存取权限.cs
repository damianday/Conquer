namespace GamePackets.Server;

[PacketInfo(Source = PacketSource.Server, ID = 612, Length = 5, Description = "更改存取权限")]
public sealed class 更改存取权限 : GamePacket
{
    [FieldAttribute(Position = 2, Length = 1)]
	public byte 行会职位;

	[FieldAttribute(Position = 3, Length = 2)]
	public ushort 权限标志;
}
